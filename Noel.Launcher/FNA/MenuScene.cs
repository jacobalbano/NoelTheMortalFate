using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Noel.Common;
using Noel.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Launcher.FNA
{
    public class MenuScene
    {
        private List<GameCard> Cards { get; }

        public MenuScene()
        {
            Cards = new List<GameCard>();
        }

        public void LoadContent(XnaApp xnaApp)
        {
            var env = NoelEnvironment.Instance;
            var gameConfig = env.Config.Get<GameDirectoryConfig>();

            var availableSpace = xnaApp.XnaCfg.InternalResolutionX * 0.95f;
            var margin = (xnaApp.XnaCfg.InternalResolutionX - availableSpace) * 0.5f;
            var cardWidth = availableSpace / CARDS_ACROSS + 1;
            var positions = Enumerable.Range(0, CARDS_ACROSS).Select(x => (cardWidth / CARDS_ACROSS) + margin + x * cardWidth).ToArray();

            int cardNum = 0;
            foreach (var season in env.Seasons)
            {
                var cfg = gameConfig.Seasons.Single(x => x.Number == season.Number);
                var texPath = Path.Combine(season.FullResourceFolderPath, cfg.TitleImagePath);
                using (var stream = File.OpenRead(texPath))
                {
                    var texture = Texture2D.FromStream(xnaApp.GraphicsDevice, stream);
                    var scale = cardWidth / texture.Width * 0.95f;
                    var card = new GameCard(texture, () => xnaApp.Quit(season.FullExecutablePath))
                    {
                        X = positions[cardNum % CARDS_ACROSS],
                        Y = (texture.Height * scale * 0.75f) * (cardNum / CARDS_ACROSS),

                        Scale = scale
                    };

                    Cards.Add(card);
                    ++cardNum;
                }
            }
        }

        public void Update()
        {
            var oldMouse = mouse;
            mouse = Mouse.GetState();

            var oldKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if (mouse.X >= 0 && mouse.Y >= 0)
            {
                var clicked = oldMouse.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released;
                if (clicked || oldMouse.X != mouse.X || oldMouse.Y != mouse.Y)
                {
                    var matrix = Matrix.Invert(GetCameraMatrix());
                    var worldCoords = Vector2.Transform(new Vector2(mouse.X, mouse.Y), matrix);

                    for (int i = Cards.Count; i --> 0;)
                    {
                        var card = Cards[i];
                        if (card.IsMouseOver(worldCoords))
                        {
                            SelectedCard = card;
                            if (clicked)
                            {
                                SelectedCard.Launch();
                                return;
                            }

                            break;
                        }
                    }
                }
            }

            var indexDelta = 0;
            if (PressEvent(oldKeyboard, Up)) indexDelta -= CARDS_ACROSS;
            if (PressEvent(oldKeyboard, Down)) indexDelta += CARDS_ACROSS;
            if (PressEvent(oldKeyboard, Left)) indexDelta--;
            if (PressEvent(oldKeyboard, Right)) indexDelta++;

            var newIndex = (int)MathHelper.Clamp(selectedIndex + indexDelta, 0, Cards.Count - 1);
            SelectedCard = Cards[newIndex];

            if (PressEvent(oldKeyboard, Keys.Enter))
                SelectedCard.Launch();
        }

        private bool PressEvent(KeyboardState oldKeyboard, params Keys[] options)
        {
            return options.Any(x => oldKeyboard.IsKeyDown(x) && keyboard.IsKeyUp(x));
        }

        private static readonly Keys[] Up = new[] { Keys.W, Keys.Up };
        private static readonly Keys[] Down = new[] { Keys.S, Keys.Down };
        private static readonly Keys[] Left = new[] { Keys.A, Keys.Left };
        private static readonly Keys[] Right = new[] { Keys.D, Keys.Right };

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GetCameraMatrix());
            foreach (var card in Cards)
                card.Draw(spriteBatch);
            spriteBatch.End();
        }

        private Matrix GetCameraMatrix()
        {
            return Matrix.CreateTranslation(camera.X, camera.Y, 0);
        }

        private GameCard SelectedCard
        {
            get => Cards[selectedIndex];
            set
            {
                var newIndex = Cards.IndexOf(value);
                if (newIndex == selectedIndex)
                    return;

                Cards[selectedIndex].IsSelected = false;
                Cards[newIndex].IsSelected = true;
                selectedIndex = newIndex;
            }
        }

        private const int CARDS_ACROSS = 2;
        private int selectedIndex;
        private MouseState mouse;
        private KeyboardState keyboard;
        private readonly Camera camera = new Camera();
    }
}
