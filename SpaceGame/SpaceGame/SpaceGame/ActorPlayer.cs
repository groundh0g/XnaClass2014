using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceGame
{
    public class ActorPlayer : ActorShooter
    {
        public static Rectangle Bounds = Rectangle.Empty;

        public Rectangle SrcRectStraight = Rectangle.Empty;
        public Rectangle SrcRectLeft = Rectangle.Empty;
        public Rectangle SrcRectRight = Rectangle.Empty;

        public static ActorPlayer Create(Rectangle bounds)
        {
            var player = new ActorPlayer();
            player.SrcRectStraight = new Rectangle(396, 79, 99, 75);
            player.SrcRectLeft = new Rectangle(258, 246, 90, 77);
            player.SrcRectRight = new Rectangle(258, 167, 90, 77);

            player.SecondsBetweenShots = 1.2;

            // player can only move around bottom half of screen
            bounds.Y = bounds.Height / 2;
            bounds.Height /= 2;

            // adjust bounds for player sprite width
            bounds.Width -= player.SrcRectStraight.Width;
            bounds.Height -= player.SrcRectStraight.Height;

            // assign bounds
            ActorPlayer.Bounds = bounds;

            // center player
            player.Location.X = bounds.Center.X - player.SrcRectStraight.Width / 2;

            return player;
        }

        public bool Update(GameTime gameTime, GamePadState gamepad)
        {
            // assume no buttons are pressed
            var speed = Vector2.Zero;
            this.SrcRect = this.SrcRectStraight;

            // get initial values from thumbsticks
            speed.X = gamepad.ThumbSticks.Left.X;
            speed.Y = -gamepad.ThumbSticks.Left.Y;

            // check DPad buttons (Up/Down)
            if (gamepad.DPad.Up == ButtonState.Pressed)
            {
                speed.Y = -1.0f;
            }
            else if (gamepad.DPad.Down == ButtonState.Pressed)
            {
                speed.Y = 1.0f;
            }

            // check DPad buttons (Left/Right)
            if (gamepad.DPad.Left == ButtonState.Pressed)
            {
                speed.X = -1.0f;
            }
            else if (gamepad.DPad.Right == ButtonState.Pressed)
            {
                speed.X = 1.0f;
            }

            // animate ship when moving left or right
            if (speed.X < 0)
            {
                this.SrcRect = this.SrcRectLeft;
            }
            else if (speed.X > 0)
            {
                this.SrcRect = this.SrcRectRight;
            }

            // move at 5 pixels per second
            this.Speed = speed * 150.0f;

            // call back to Actor.Update for location calculation and cooldown
            base.Update(gameTime);

            // keep ship in bounds
            var bounds = ActorPlayer.Bounds;
            if (!Rectangle.Empty.Equals(bounds))
            {
                if (this.Location.X < bounds.Left)
                {
                    this.Location.X = bounds.Left;
                }

                if (this.Location.X > bounds.Right)
                {
                    this.Location.X = bounds.Right;
                }

                if (this.Location.Y < bounds.Top)
                {
                    this.Location.Y = bounds.Top;
                }

                if (this.Location.Y > bounds.Bottom)
                {
                    this.Location.Y = bounds.Bottom;
                }
            }

            // try to shoot?
            if (gamepad.Buttons.A == ButtonState.Pressed)
            {
                this.TryToShoot();
            }

            // check for collision with enemy bullet
            if (ActorBullet.Touching(this, 1))
            {
                this.Color = Color.Red;
            }

            // check for collision with enemy ship
            if (ActorEnemy.Touching(this))
            {
                this.Color = Color.Red;
            }

            // check for collision with rock
            if (ActorRock.Touching(this))
            {
                this.Color = Color.Red;
            }

            return this.Color != Color.Red;
        }
    }
}
