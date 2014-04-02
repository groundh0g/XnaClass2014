using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MM3K.Screens
{
    public static class ScreenManager
    {
        private static BaseScreen _CurrentScreen;
        public static BaseScreen CurrentScreen
        {
            get { return _CurrentScreen; }
            set
            {
                if (_CurrentScreen != null)
                {
                    _CurrentScreen.Hiding();
                }
                _CurrentScreen = value;
                _CurrentScreen.Showing();
            }
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
