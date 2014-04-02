using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class ActorShooter : Actor
    {
        public double ShotCoolDown = 0.0;
        public double SecondsBetweenShots = 3.0;

        public override void Update(GameTime gameTime)
        {
            this.ShotCoolDown -= gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public void TryToShoot()
        {
            ActorBullet.AddBullet(this);
        }

    }
}
