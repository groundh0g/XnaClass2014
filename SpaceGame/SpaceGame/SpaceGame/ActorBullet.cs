using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class ActorBullet : Actor
    {
        private static List<ActorBullet> _bullets = new List<ActorBullet>();

        public static void RemoveAllBullets() { ActorBullet._bullets.Clear(); }

        public static void UpdateAllBullets(GameTime gameTime)
        {
            CleanHouse();
            foreach (var shot in _bullets)
            {
                shot.Update(gameTime);
            }
        }

        public static bool Touching(Actor actor, int direction)
        {
            var result = false;
            var rectActor = actor.ScreenRect;
            if (direction < 0)
            {

                foreach (var bullet in _bullets)
                {
                    if (bullet.Speed.Y < 0)
                    {
                        var rectBullet = bullet.ScreenRect;
                        if (rectActor.Intersects(rectBullet))
                        {
                            bullet.Color = Color.Transparent;
                            result = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var bullet in _bullets)
                {
                    if (bullet.Speed.Y > 0)
                    {
                        var rectBullet = bullet.ScreenRect;
                        if (rectActor.Intersects(rectBullet))
                        {
                            bullet.Color = Color.Transparent;
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static void DrawAllBullets(GameTime gameTime, SpriteBatch batch, Texture2D sprites)
        {
            foreach (var shot in _bullets)
            {
                shot.Draw(gameTime, batch, sprites);
            }
        }

        public static void AddBullet(ActorShooter shooter)
        {
            if (shooter is ActorPlayer)
            {
                AddBullet(shooter as ActorPlayer);
            }
            else
            {
                AddBullet(shooter as ActorEnemy);
            }
        }

        private static void AddBullet(ActorEnemy enemy)
        {
            if (enemy != null && enemy.ShotCoolDown <= 0)
            {
                var shot = new ActorBullet();
                shot.SrcRect = new Rectangle(497, 2, 9, 33);
                shot.Location.X = enemy.Location.X
                    + enemy.SrcRect.Width / 2
                    - shot.SrcRect.Width / 2;
                shot.Location.Y = enemy.Location.Y
                    + enemy.SrcRect.Height
                    - shot.SrcRect.Height;
                shot.Speed.Y = 200.0f;
                _bullets.Add(shot);
                enemy.ShotCoolDown = enemy.SecondsBetweenShots;
            }
        }

        private static void AddBullet(ActorPlayer player)
        {
            if (player != null && player.ShotCoolDown <= 0)
            {
                var shot = new ActorBullet();
                shot.SrcRect = new Rectangle(497, 37, 9, 33);
                shot.Location.X = player.Location.X
                    + player.SrcRect.Width / 2
                    - shot.SrcRect.Width / 2;
                shot.Location.Y = player.Location.Y;
                shot.Speed.Y = -200.0f;
                _bullets.Add(shot);
                player.ShotCoolDown = player.SecondsBetweenShots;
            }
        }

        private static void CleanHouse()
        {
            int i = 0;
            while (i < _bullets.Count)
            {
                if (_bullets[i].Location.Y > ActorPlayer.Bounds.Bottom)
                {
                    // remove enemy bullet when off the screen
                    _bullets.RemoveAt(i);
                }
                else if (_bullets[i].Location.Y < -_bullets[i].SrcRect.Height)
                {
                    // remove player bullet when off the screen
                    _bullets.RemoveAt(i);
                }
                else if (_bullets[i].Color == Color.Transparent)
                {
                    // remove any bullet when it hits something
                    _bullets.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
