using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    class Mine : Enemy
    {
        private Player player;

        public Mine(Texture2D texture, float X, float Y, Player player) : base(texture, X, Y, 6f, 0.3f)
        {
            this.player = player;
        }

        public override void Update(GameWindow window)
        {
            if (player.PlayerPosX > vector.X)
                vector.X ++;
            else if (player.PlayerPosX < vector.X)
                vector.X --;

            if (player.PlayerPosY > vector.Y)
                vector.Y ++;
            else if (player.PlayerPosY < vector.Y)
                vector.Y --;



            //vector.X += speed.X;
            //    if (vector.X > window.ClientBounds.Width - texture.Width || vector.X< 0)
            //    {
            //        speed.X *= -1;
            //    }

            //vector.Y += speed.Y;
            //    if (vector.Y > window.ClientBounds.Height)
            //    {
            //        isAlive = false;
            //    }
        }
    }
}
