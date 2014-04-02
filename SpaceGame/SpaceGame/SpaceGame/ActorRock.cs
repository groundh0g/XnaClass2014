using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class ActorRock : Actor
    {
        public static double AddRockCoolDown = 3.0;

        private static Random _rand = new Random();
        private static List<ActorRock> _rocks = new List<ActorRock>();

        public static void RemoveAllRocks() { ActorRock._rocks.Clear(); }

        public static void TryToAddRock(GameTime gameTime)
        {
            CleanHouse();
            ActorRock.AddRockCoolDown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (ActorRock.AddRockCoolDown <= 0)
            {
                var rock = new ActorRock();
                rock.Speed.Y = (float)_rand.NextDouble() * 25.0f + 10.0f;
                rock.Location.X = _rand.Next(ActorPlayer.Bounds.Width - rock.SrcRect.Width);

                if (_rand.Next(4) < 3)
                {
                    // small rock
                    rock.SrcRect = new Rectangle(350, 249, 44, 42);
                }
                else
                {
                    // big rock
                    rock.SrcRect = new Rectangle(258, 2, 136, 111);
                }

                _rocks.Add(rock);
                ActorRock.AddRockCoolDown = _rand.NextDouble() * 3.0 + 5.0;
            }
        }

        public static void UpdateAllRocks(GameTime gameTime)
        {
            foreach (var rock in _rocks)
            {
                rock.Update(gameTime);
                // check for collision with player bullet
                if (ActorBullet.Touching(rock, -1))
                {
                    rock.Color = Color.Transparent;
                }
            }
        }

        public static void DrawAllRocks(GameTime gameTime, SpriteBatch batch, Texture2D sprites)
        {
            foreach (var rock in _rocks)
            {
                rock.Draw(gameTime, batch, sprites);
            }
        }

        private static void CleanHouse()
        {
            int i = 0;
            while (i < _rocks.Count)
            {
                if (_rocks[i].Location.Y > ActorPlayer.Bounds.Bottom)
                {
                    // remove rock when off the screen
                    _rocks.RemoveAt(i);
                }
                else if (_rocks[i].Color == Color.Transparent)
                {
                    // remove rock when destroyed
                    _rocks.RemoveAt(i);
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

            foreach (var rock in _rocks)
            {
                var rectRock = rock.ScreenRect;
                if (rectActor.Intersects(rectRock))
                {
                    rock.Color = Color.Transparent;
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
