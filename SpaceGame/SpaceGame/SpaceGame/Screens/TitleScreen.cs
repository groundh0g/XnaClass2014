using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Screens
{
    public class TitleScreen : Screen
    {
        private SpriteFont font;
        private Vector2 locStartText = Vector2.Zero;
        private double elapsedBlink = 0.0;
        Texture2D sprites;

        public TitleScreen(Game parent) : base(parent) {
            locStartText.X = parent.GraphicsDevice.Viewport.Bounds.Width / 2;
            locStartText.Y = parent.GraphicsDevice.Viewport.Bounds.Height / 2;
        }

        private bool isCentered = false;
        private const string PRESS_START = "{Please, Press Start!}";
        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (isCentered == false)
            {
                locStartText.X -= font.MeasureString(PRESS_START).X / 2.0f;
                locStartText.Y -= font.MeasureString(PRESS_START).Y / 2.0f;
                isCentered = true;
            }

            batch.GraphicsDevice.Clear(Color.Purple);
            ActorBackground.Draw(gameTime, batch, sprites);
            batch.DrawString(font, "Space Shooter!", Vector2.One * 100.0f, Color.White);
            if ((int)elapsedBlink % 2 == 1)
            {
                batch.DrawString(font, PRESS_START, locStartText, Color.White);
            }
            base.Draw(gameTime, batch);
        }

        bool wasBackPressed = true;
        public override void Update(GameTime gameTime, GamePadState padState)
        {
            ActorBackground.Update(gameTime);

            elapsedBlink += gameTime.ElapsedGameTime.TotalSeconds * 2.0;
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

        public override void Showing()
        {
            sprites = Parent.Content.Load<Texture2D>("sprites");
            font = Parent.Content.Load<SpriteFont>("FontTitle");
            base.Showing();
        }
    }
}
