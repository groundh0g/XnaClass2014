using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class ActorEnemy : ActorShooter
    {
        public static double AddEnemyCoolDown = 3.0;

        private static Random _rand = new Random();
        private static List<ActorEnemy> _enemies = new List<ActorEnemy>();

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.TryToShoot();
        }

        public static void RemoveAllEnemies() { ActorEnemy._enemies.Clear();  }

        public static void TryToAddEnemy(GameTime gameTime)
        {
            CleanHouse();
            ActorEnemy.AddEnemyCoolDown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (ActorEnemy.AddEnemyCoolDown <= 0)
            {
                var enemy = new ActorEnemy();
                enemy.SrcRect = new Rectangle(258, 115, 98, 50);
                enemy.ShotCoolDown = _rand.NextDouble() * 3.0 + 1.0;

                if (_rand.Next(2) == 1)
                {
                    // from the left
                    enemy.Speed.X = (float)_rand.NextDouble() * 25.0f + 10.0f;
                    enemy.Location.Y = (float)_rand.NextDouble() * 150.0f;
                }
                else
                {
                    // from the right
                    enemy.Speed.X = -((float)_rand.NextDouble() * 25.0f + 10.0f);
                    enemy.Location.X = ActorPlayer.Bounds.Right;
                    enemy.Location.Y = (float)_rand.NextDouble() * 150.0f;
                }

                _enemies.Add(enemy);
                ActorEnemy.AddEnemyCoolDown = _rand.NextDouble() * 3.0 + 3.0;
            }
        }

        public static void UpdateAllEnemies(GameTime gameTime)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime);
                // check for collision with player bullet
                if (ActorBullet.Touching(enemy, -1))
                {
                    enemy.Color = Color.Transparent;
                }
            }
        }

        public static void DrawAllEnemies(GameTime gameTime, SpriteBatch batch, Texture2D sprites)
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw(gameTime, batch, sprites);
            }
        }

        private static void CleanHouse()
        {
            int i = 0;
            while (i < _enemies.Count)
            {
                if (_enemies[i].Location.Y > ActorPlayer.Bounds.Bottom)
                {
                    // remove rock when off the screen
                    _enemies.RemoveAt(i);
                }
                else if (_enemies[i].Color == Color.Transparent)
                {
                    // remove rock when destroyed
                    _enemies.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public static bool Touching(Actor actor)
        {
            var result = false;
            var rectActor = actor.ScreenRect;

            foreach (var enemy in _enemies)
            {
                var rectEnemy = enemy.ScreenRect;
                if (rectActor.Intersects(rectEnemy))
                {
                    enemy.Color = Color.Transparent;
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
