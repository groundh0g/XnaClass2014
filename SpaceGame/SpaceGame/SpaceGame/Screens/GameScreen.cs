using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame.Screens
{
    public class GameScreen : Screen
    {
        Texture2D sprites;
        ActorPlayer player;

        public GameScreen(Game parent) : base(parent) { }

        Color COLOR_FADED = new Color(128, 128, 128, 128);
        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            ActorBackground.Draw(gameTime, batch, sprites);
            ActorRock.DrawAllRocks(gameTime, batch, sprites);
            ActorBullet.DrawAllBullets(gameTime, batch, sprites);
            player.Draw(gameTime, batch, sprites);
            ActorEnemy.DrawAllEnemies(gameTime, batch, sprites);

            if (isPaused)
            {
                batch.Draw(sprites, Parent.GraphicsDevice.Viewport.Bounds, new Rectangle(10, 10, 20, 20), COLOR_FADED);
            }

            base.Draw(gameTime, batch);
        }

        private bool wasStartPressed = true;
        private bool isPaused = false;

        public override void Update(GameTime gameTime, GamePadState padState)
        {
            // Allows the game to exit

            bool isStartPressed = padState.Buttons.Start == ButtonState.Pressed;

            if (padState.Buttons.Back == ButtonState.Pressed)
            {
                // end game
                ScreenManager.SetCurrentScreen(new TitleScreen(Parent));
            }
            else if (!wasStartPressed && isStartPressed)
            {
                isPaused = !isPaused;
            }

            wasStartPressed = isStartPressed;

            if (isPaused)
            {
                return;
            }

            ActorBackground.Update(gameTime);

            if (!player.Update(gameTime, padState))
            {
                ScreenManager.SetCurrentScreen(new GameOverScreen(Parent));
            }
            
            ActorBullet.UpdateAllBullets(gameTime);
            ActorRock.UpdateAllRocks(gameTime);
            ActorEnemy.UpdateAllEnemies(gameTime);

            ActorEnemy.TryToAddEnemy(gameTime);
            ActorRock.TryToAddRock(gameTime);

            base.Update(gameTime, padState);
        }

        public override void Showing()
        {
            // create our player
            sprites = Parent.Content.Load<Texture2D>("sprites");
            player = ActorPlayer.Create(Parent.GraphicsDevice.Viewport.Bounds);
            ActorRock.RemoveAllRocks();
            ActorBullet.RemoveAllBullets();
            ActorEnemy.RemoveAllEnemies();
        }
    }
}
