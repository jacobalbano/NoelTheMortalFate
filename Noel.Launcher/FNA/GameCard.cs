using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noel.Launcher.FNA
{
    public class GameCard
    {
        private Texture2D Texture { get; }
        public string FullExecutablePath { get; }
        public Action Launch { get; }

        public bool IsSelected { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Scale { get; set; }
        public float Angle { get; set; }

        public GameCard(Texture2D texture2D, Action launch)
        {
            Texture = texture2D;
            Launch = launch;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TowardsWithDecay(ref scaleAlpha, IsSelected ? 0.01f : 0, 0.4f);
            var sine = (float)(-Math.Cos(Math.PI * (t += 0.04f)) / 2 + .5);
            spriteBatch.Draw(Texture, new Vector2(X, Y), null, Color.White, Angle, new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f), Scale + (scaleAlpha * sine), SpriteEffects.None, 0);
        }

        internal bool IsMouseOver(Vector2 worldCoords)
        {
            var bounds = new Rectangle
            {
                X = (int)(X - (Texture.Width * Scale) / 2),
                Y = (int)(Y - (Texture.Height * Scale) / 2),
                Width = (int)(Texture.Width * Scale),
                Height = (int)(Texture.Height * Scale)
            };

            return bounds.Contains((int)worldCoords.X, (int)worldCoords.Y);
        }

        private static void TowardsWithDecay(ref float target, float to, float amount = 0.1f)
        {
            target += (to - target) * amount;
        }

        private float scaleAlpha;
        private float t;
    }
}
