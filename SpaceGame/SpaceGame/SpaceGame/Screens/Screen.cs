using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Screens
{
    public class Screen
    {
        public Game Parent { get; set; }

        public Rectangle Bounds
        {
            get
            {
                return Parent.GraphicsDevice.Viewport.Bounds;
            }
        }

        public Screen(Game parent)
        {
            this.Parent = parent;
        }

        //public virtual void Update(GameTime gameTime)
        //{
        //}

        public virtual void Update(GameTime gameTime, GamePadState padState)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
        }

        public virtual void Showing()
        {
        }
    }
}
