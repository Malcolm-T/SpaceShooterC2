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

        public Mine(Texture2D texture, float X, float Y, GameWindow window, Player player) : base(texture, 0, 0, 6f, 0.3f, window)
        {
            this.player = player;
            Random r = new Random();
            int sida = r.Next(0, 4);

            switch (sida)
            {
                case 0:
                    vector.X = r.Next(0, window.ClientBounds.Width - texture.Width);
                    vector.Y = -texture.Height;
                    break;
                case 1:
                    vector.X = window.ClientBounds.Width;
                    vector.Y = r.Next(0, window.ClientBounds.Height - texture.Height);
                    break;
                case 2:
                    vector.X = r.Next(0, window.ClientBounds.Width - texture.Width);
                    vector.Y = window.ClientBounds.Height;
                    break;
                case 3:
                    vector.X = -texture.Width;
                    vector.Y = r.Next(0, window.ClientBounds.Height - texture.Height);
                    break;
            }
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            if (player.X > vector.X)
                vector.X ++;
            else if (player.X < vector.X)
                vector.X --;

            if (player.Y > vector.Y)
                vector.Y ++;
            else if (player.Y < vector.Y)
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
