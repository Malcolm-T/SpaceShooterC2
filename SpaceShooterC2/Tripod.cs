using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Tripod : Enemy
    {
        public Tripod(Texture2D texture, float X, float Y, GameWindow window) : base(texture, X, Y, 0f, 3f, window)
        {
            Random r = new Random();
            int sida = r.Next(0, 4);

            switch (sida)
            {
                case 0: // Uppifrån
                    vector = new Vector2(r.Next(0, window.ClientBounds.Width - texture.Width), -texture.Height);
                    speed = new Vector2(0, 3f);
                    break;
                case 1: // Höger
                    vector = new Vector2(window.ClientBounds.Width, r.Next(0, window.ClientBounds.Height - texture.Height));
                    speed = new Vector2(-3f, 0);
                    break;
                case 2: // Nedifrån
                    vector = new Vector2(r.Next(0, window.ClientBounds.Width - texture.Width), window.ClientBounds.Height);
                    speed = new Vector2(0, -3f);
                    break;
                case 3: // Vänster
                    vector = new Vector2(-texture.Width, r.Next(0, window.ClientBounds.Height - texture.Height));
                    speed = new Vector2(3f, 0);
                    break;
            }
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            vector += speed; // Flytta enligt hastighets-vektorn vi satte i konstruktorn

            if (vector.X < -texture.Width || vector.X > window.ClientBounds.Width ||
                vector.Y < -texture.Height || vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }
    }
}
