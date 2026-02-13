using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Item : PhysicalObject
    {
        double timeToDie;

        public Item(float X, float Y, GameTime gameTime) : base(null, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
        }

        public void Update(GameTime gameTime)
        {
            if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
            {
                isAlive = false;
            }
        }
    }
}
