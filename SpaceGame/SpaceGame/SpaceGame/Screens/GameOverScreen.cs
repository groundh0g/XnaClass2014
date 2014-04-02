using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Screens
{
    public class GameOverScreen : Screen
    {
        private SpriteFont font;

        public GameOverScreen(Game parent) : base(parent) { }

        bool wasBackPressed = true;
        public override void Update(GameTime gameTime, GamePadState padState)
        {
            bool isBackPressed = padState.Buttons.Back == ButtonState.Pressed;
            if (padState.Buttons.Start == ButtonState.Pressed)
            {
                ScreenManager.SetCurrentScreen(new GameScreen(Parent));
            }
            else if (!wasBackPressed && isBackPressed)
            {
                Parent.Exit();
            }
            wasBackPressed = isBackPressed;
            base.Update(gameTime, padState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.GraphicsDevice.Clear(Color.Green);
            batch.DrawString(font, "Game Over!  :(", Vector2.One * 100.0f, Color.White);
            base.Draw(gameTime, batch);
        }

        public override void Showing()
        {
            font = Parent.Content.Load<SpriteFont>("FontTitle");
            base.Showing();
        }
    }
}
