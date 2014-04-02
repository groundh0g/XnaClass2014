using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MM3K.Screens
{
    public class GameScreen : BaseScreen
    {
        Texture2D texTile;
        Texture2D texBaby;
        Texture2D texBaddie;
        Texture2D texBanana;
        Texture2D texMonkey;
        Rectangle rectGround = new Rectangle(128, 128, 128, 128);

        public const float GROUND_LEVEL_Y = 298;
        public const float GRAVITY = 600.0f;
        public float accelerationY = 0.0f;

        public GameScreen(Game parent)
            : base(parent)
        {
        }


        Vector2 dBaby = new Vector2(0, 0);

        public bool wasAPressed = true;
        public override void Update(GameTime gameTime, GamePadState padState)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (padState.DPad.Left == ButtonState.Pressed)
            {
                locMonkey.X -= 100.0f * elapsed;
            }
            else if (padState.DPad.Right == ButtonState.Pressed)
            {
                locMonkey.X += 100.0f * elapsed;
            }

            var isAPressed = 
                padState.Buttons.A == ButtonState.Pressed &&
                accelerationY == 0;
            if (!wasAPressed && isAPressed)
            {
                accelerationY = -450;
            }

            wasAPressed = isAPressed;
            locMonkey.Y += accelerationY * elapsed;
            accelerationY += GRAVITY * elapsed;
            if (locMonkey.Y > GROUND_LEVEL_Y)
            {
                locMonkey.Y = GROUND_LEVEL_Y;
                accelerationY = 0;
            }

            var rectBaby = new Rectangle((int)locBaby.X, (int)locBaby.Y, texBaby.Width, texBaby.Height);
            var rectMonkey = new Rectangle((int)locMonkey.X, (int)locMonkey.Y, texMonkey.Width, texMonkey.Height);

            if (rectBaby.Intersects(rectMonkey))
            {
                dBaby.X = -6;
            }

            locBaby.X += dBaby.X;
            locBaby.Y += dBaby.Y;

            base.Update(gameTime, padState);
        }

        Vector2 locMonkey = new Vector2(0, GROUND_LEVEL_Y);
        Vector2 locBaby = new Vector2(200, 375);
        Vector2 locBaddie = new Vector2(300, 360);
        Vector2 locBanana = new Vector2(400, 390);

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            Vector2 locTile = new Vector2(-32, Bounds.Height - 128);
            while (locTile.X < Bounds.Width)
            {
                batch.Draw(texTile, locTile, rectGround, Color.White);
                locTile.X += 90;
            }

            batch.Draw(texMonkey, locMonkey, Color.White);
            batch.Draw(texBaby, locBaby, Color.White);
            batch.Draw(texBaddie, locBaddie, Color.White);
            batch.Draw(texBanana, locBanana, Color.White);
            //System.Diagnostics.Debug.WriteLine(locMonkey.ToString());

            base.Draw(gameTime, batch);
        }

        public override void Showing()
        {
            texBaby = Parent.Content.Load<Texture2D>("baby");
            texBaddie = Parent.Content.Load<Texture2D>("baddie");
            texBanana = Parent.Content.Load<Texture2D>("bananas");
            texTile = Parent.Content.Load<Texture2D>("mousers");
            texMonkey = Parent.Content.Load<Texture2D>("monkey");
            base.Showing();
        }

        public override void Hiding()
        {
            base.Hiding();
        }
    }
}
