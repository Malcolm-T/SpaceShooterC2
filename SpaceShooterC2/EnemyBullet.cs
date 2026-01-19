using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class EnemyBullet : PhysicalObject
    {
        public EnemyBullet(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {

        }

        public void Update()
        {
            vector.X += speed.X/2;
            vector.Y += speed.Y/2;

            if (vector.Y < -50 || vector.Y > 2000 || vector.X < -50 || vector.X > 2000)
                isAlive = false;


            //vector.Y -= speed.Y;
            //if (vector.Y < 0)
            //    isAlive = false;
        }
    }
}
