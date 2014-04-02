using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame
{
    public class ActorBackground : Actor
    {
        private static Rectangle srcRect = new Rectangle(2, 2, 254, 256);

        private static int progress = 0;

        public static void Update(GameTime gameTime)
        {
            progress = (int)(100.0 * gameTime.TotalGameTime.TotalSeconds) % srcRect.Height;
        }

        public static void Draw(GameTime gameTime, SpriteBatch batch, Texture2D sprites)
        {
            var bounds = batch.GraphicsDevice.Viewport.Bounds;
            var loc = Vector2.Zero;
            loc.Y += progress;
            if(loc.Y > 0) {
                loc.Y -= srcRect.Height;
            }
            while (loc.Y < bounds.Bottom)
            {
                loc.X = 0;
                while (loc.X < bounds.Right)
                {
                    batch.Draw(sprites, loc, Color.White);
                    loc.X += srcRect.Width;
                }
                loc.Y += srcRect.Height;
            }
        }

    }
}
