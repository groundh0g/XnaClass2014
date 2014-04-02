using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame
{
    public class Actor
    {
        // common properties for all Actors
        public Vector2 Location = Vector2.Zero;
        public Rectangle SrcRect = Rectangle.Empty;
        public Color Color = Color.White;

        // pixels per second
        public Vector2 Speed = Vector2.Zero;

        // on-screen rectangle
        public Rectangle ScreenRect
        {
            get
            {
                var rect = this.SrcRect;
                rect.X = (int)this.Location.X;
                rect.Y = (int)this.Location.Y;
                return rect;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Location += this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch, Texture2D sprites)
        {
            batch.Draw(sprites, this.Location, this.SrcRect, this.Color);
        }
    }
}
