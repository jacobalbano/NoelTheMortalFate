using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Noel.Common.Config;
using Noel.Common.Data;
using Noel.Launcher.FNA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Launcher
{
    public class XnaApp : Game
    {
        public string ChosenGamePath { get; private set; }
        public XnaAppConfig XnaCfg { get; }

        public XnaApp(XnaAppConfig xnaCfg)
        {
            XnaCfg = xnaCfg;
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = xnaCfg.InternalResolutionX,
                PreferredBackBufferHeight = xnaCfg.InternalResolutionY,
                SynchronizeWithVerticalRetrace = true
            };

            Window.AllowUserResizing = true;
            IsMouseVisible = true;
            
            Window.ClientSizeChanged += (sender, e) =>
            {
                xnaCfg.WindowResolutionX = Window.ClientBounds.Width;
                xnaCfg.WindowResolutionY = Window.ClientBounds.Height;
            };

            scene = new MenuScene();
        }

        public void Quit(string gamePath)
        {
            ChosenGamePath = gamePath;
            Exit();
        }

        #region XNA overrides
        protected override void Initialize()
        {
            base.Initialize();
            Target = new RenderTarget2D(GraphicsDevice, XnaCfg.WindowResolutionX, XnaCfg.WindowResolutionY);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            scene.LoadContent(this);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            scene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //  Draw entities to the buffer
            {
                GraphicsDevice.SetRenderTarget(Target);
                GraphicsDevice.Clear(Color.Black);
                scene.Draw(gameTime, spriteBatch);
            }

            //  Draw buffer to screen, transforming it to fit the window size
            {
                GraphicsDevice.SetRenderTarget(null);

                //  order of operations is important here for some reason;
                //  this has to happen after SetRenderTarget(null)
                var res = GetVirtualRes(
                    Target.Width,
                    Target.Height,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height
                );

                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, res);
                spriteBatch.Draw(Target, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }
        }

        #endregion

        private static Matrix GetVirtualRes(float targetWidth, float targetHeight, float currentWidth, float currentHeight)
        {
            float windowRatio = currentWidth / currentHeight;
            float viewRatio = targetWidth / targetHeight;
            float posX = 0, posY = 0;

            if (windowRatio < viewRatio) posY = (1 - windowRatio / viewRatio) / 2.0f;
            else posX = (1 - viewRatio / windowRatio) / 2.0f;

            var scale = Math.Min(currentWidth / targetWidth, currentHeight / targetHeight);
            return Matrix.CreateScale(scale, scale, 1) * Matrix.CreateTranslation(posX * currentWidth, posY * currentHeight, 0);
        }

        private SpriteBatch spriteBatch;
        private RenderTarget2D Target;
        private readonly GraphicsDeviceManager Graphics;
        private readonly MenuScene scene;
    }
}
