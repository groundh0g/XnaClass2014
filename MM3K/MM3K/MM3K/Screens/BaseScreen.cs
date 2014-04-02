using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MM3K.Screens
{
    public abstract class BaseScreen
    {
        public Game Parent { get; set; }

        public Rectangle Bounds
        {
            get
            {
                return Parent.GraphicsDevice.Viewport.Bounds;
            }
        }

        public BaseScreen(Game parent)
        {
            this.Parent = parent;
        }

        public virtual void Showing() { }
        public virtual void Hiding() { }
        
        public virtual void Update(GameTime gameTime, GamePadState padState)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
        }
    }
}
