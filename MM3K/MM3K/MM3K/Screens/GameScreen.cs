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
        Texture2D texShooter;
        Texture2D texPoo;

        Rectangle rectGround = new Rectangle(128, 128, 128, 128);

        Rectangle[] rectRunFrames = 
        {
            new Rectangle(59 + 0 * 64, 64, 64, 64),
            new Rectangle(59 + 1 * 64, 64, 64, 64),
            new Rectangle(59 + 2 * 64, 64, 64, 64),
            new Rectangle(59 + 3 * 64, 64, 64, 64),
            new Rectangle(59 + 4 * 64, 64, 64, 64),
            new Rectangle(59 + 5 * 64, 64, 64, 64),
        };
        int currentRunFrame = 0;
        const double MAX_RUN_FRAME_DURATION = 0.1;
        double elapsedRunFrameDuration = MAX_RUN_FRAME_DURATION;

        Rectangle rectStill = new Rectangle(59 + 5 * 64, 0, 64, 64);

        Rectangle[] rectJumpFrames = 
        {
            new Rectangle(59 + 0 * 64, 0, 64, 64),
            new Rectangle(59 + 1 * 64, 0, 64, 64),
        };

        Rectangle rectShootBottom = new Rectangle(0, 0, 60, 68);
        Rectangle rectShootTop = new Rectangle(60, 0, 88, 68);

        Vector2 originShootTopRight = new Vector2(25, 53);
        Vector2 originShootTopLeft = new Vector2(88 - 25, 53);

        public const float GROUND_LEVEL_Y = 298 + 56;
        public const float GRAVITY = 600.0f;
        public float accelerationY = 0.0f;
        public Vector2 accelerationPoo = Vector2.Zero;

        public GameScreen(Game parent)
            : base(parent)
        {
        }


        Vector2 dBaby = new Vector2(0, 0);

        public bool wasAPressed = true;
        public bool isRunning = false;
        public bool isFacingLeft = false;
        public bool isAiming = false;
        public float aimRadians = 0.0f;

        public override void Update(GameTime gameTime, GamePadState padState)
        {
            if (padState.Buttons.Back == ButtonState.Pressed)
            {
                ScreenManager.CurrentScreen = new TitleScreen(Parent);
            }
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (padState.Triggers.Right > 0.5f)
            {
                locPoo = locMonkey + (isFacingLeft ? originShootTopLeft + new Vector2(-32, -45) : originShootTopRight + new Vector2(5, -45));
                accelerationPoo = new Vector2(
                    isFacingLeft ? -10.0f : 10.0f,
                    aimRadians * 10.0f);
            }

            locPoo += accelerationPoo;

            isAiming = padState.ThumbSticks.Right.Y != 0.0f || padState.ThumbSticks.Right.X != 0.0f;
            if (isAiming)
            {
                aimRadians = padState.ThumbSticks.Right.Y * 1.0f;
                aimRadians = Math.Max(aimRadians, -0.870878f);
                aimRadians = Math.Min(aimRadians, 0.6582773f);
                System.Diagnostics.Debug.WriteLine(aimRadians);
                return;
            }

            isRunning = false;
            float speed = padState.ThumbSticks.Left.X;
            if (padState.DPad.Left == ButtonState.Pressed)
            {
                speed = -1.0f;
            }
            else if (padState.DPad.Right == ButtonState.Pressed)
            {
                speed = 1.0f;
            }

            locMonkey.X += speed * 150.0f * elapsed;
            isRunning = speed != 0.0f;
            if (speed != 0.0f)
            {
                isFacingLeft = speed < 0.0f;
            }

            elapsedRunFrameDuration -= elapsed;
            if (elapsedRunFrameDuration <= 0.0)
            {
                currentRunFrame = (currentRunFrame + 1) % rectRunFrames.Length;
                elapsedRunFrameDuration = (2.0f - Math.Abs(speed)) * MAX_RUN_FRAME_DURATION;
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
            if (locMonkey.Y >= GROUND_LEVEL_Y)
            {
                locMonkey.Y = GROUND_LEVEL_Y;
                accelerationY = 0;
            } 

            var rectBaby = new Rectangle((int)locBaby.X, (int)locBaby.Y, texBaby.Width, texBaby.Height);
            var rectMonkey = new Rectangle((int)locMonkey.X, (int)locMonkey.Y, 64, 64);

            if (rectBaby.Intersects(rectMonkey))
            {
                dBaby.X = -6;
            }

            locBaby.X += dBaby.X;
            locBaby.Y += dBaby.Y;

            base.Update(gameTime, padState);
        }

        Vector2 locMonkey = new Vector2(0, GROUND_LEVEL_Y);
        Vector2 locPoo = Vector2.Zero;
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

            if (isRunning && accelerationY == 0)
            {
                batch.Draw(
                    texMonkey, // texture
                    locMonkey, // location 
                    rectRunFrames[currentRunFrame], // source rectangle
                    Color.White, // tint
                    0.0f, // rotation
                    Vector2.Zero, // origin
                    1.0f, // scale
                    isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, // SpriteEffect
                    0.0f); // depth
            }
            else
            {
                if (accelerationY < 0)
                {
                    batch.Draw(
                        texMonkey,
                        locMonkey,
                        rectJumpFrames[0],
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);
                    isRunning = false;
                }
                else if (accelerationY > 0)
                {
                    batch.Draw(
                        texMonkey,
                        locMonkey,
                        rectJumpFrames[1],
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);
                    isRunning = false;
                }
                else if (isAiming)
                {
                    batch.Draw(
                        texShooter,
                        locMonkey + (isFacingLeft ? originShootTopLeft + new Vector2(-32, 0) : originShootTopRight + new Vector2(5, 0)),
                        rectShootTop,
                        Color.White,
                        aimRadians,
                        (isFacingLeft ? originShootTopLeft : originShootTopRight),
                        1.0f,
                        isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);
                    batch.Draw(
                        texShooter,
                        locMonkey,
                        rectShootBottom,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);
                }
                else
                {
                    batch.Draw(
                        texMonkey,
                        locMonkey,
                        rectStill,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        0.0f);
                }
            }
            //batch.Draw(texMonkey, locMonkey, Color.White);
            batch.Draw(texBaby, locBaby, Color.White);
            batch.Draw(texBaddie, locBaddie, Color.White);
            batch.Draw(texBanana, locBanana, Color.White);
            batch.Draw(texPoo, locPoo, Color.White);
            //System.Diagnostics.Debug.WriteLine(locMonkey.ToString());

            base.Draw(gameTime, batch);
        }

        public override void Showing()
        {
            texBaby = Parent.Content.Load<Texture2D>("baby");
            texBaddie = Parent.Content.Load<Texture2D>("baddie");
            texBanana = Parent.Content.Load<Texture2D>("bananas");
            texTile = Parent.Content.Load<Texture2D>("mousers");
            texMonkey = Parent.Content.Load<Texture2D>("Standupandrun");
            texShooter = Parent.Content.Load<Texture2D>("shooter");
            texPoo = Parent.Content.Load<Texture2D>("poo");
            base.Showing();
        }

        public override void Hiding()
        {
            base.Hiding();
        }
    }
}
