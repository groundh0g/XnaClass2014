using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Screens
{
    public class ScreenManager
    {
        public static Screen CurrentScreen { get; private set; }

        public static void SetCurrentScreen(Screen screen)
        {
            CurrentScreen = screen;
            screen.Showing();
        }

        public static void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            CurrentScreen.Draw(gameTime, batch);
            batch.End();
        }

        public static void Update(GameTime gameTime)
        {
            var padState = GamePadEx.GetState(PlayerIndex.One);
            CurrentScreen.Update(gameTime, padState);
        }
    }
}
