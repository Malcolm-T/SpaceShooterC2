using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Shooter : Enemy
    {
        private Player player;
        Texture2D bulletTexture;
        List<EnemyBullet> bullets;
        double timeSinceLastBullet = 0;
        float rotation = 0f;

        int sida;
        int targetPos;
        public Shooter(Texture2D texture, float X, float Y, GameWindow window, Player player, Texture2D bulletTexture) : base(texture, X, Y, 6f, 0.3f, window)
        {
            this.player = player;
            bullets = new List<EnemyBullet>();
            this.bulletTexture = bulletTexture;


            Random r = new Random();
            targetPos = r.Next(50, 150);
            sida = r.Next(0, 4);


            switch (sida)
            {
                case 0: // Uppifrån
                    vector = new Vector2(r.Next(0, window.ClientBounds.Width - texture.Width), -texture.Height);
                    speed = new Vector2(0, 3f);
                    break;
                case 1: // Höger
                    vector = new Vector2(window.ClientBounds.Width, r.Next(0, window.ClientBounds.Height - texture.Height));
                    speed = new Vector2(3f, 0);
                    break;
                case 2: // Nedifrån
                    vector = new Vector2(r.Next(0, window.ClientBounds.Width - texture.Width), window.ClientBounds.Height);
                    speed = new Vector2(0, 3f);
                    break;
                case 3: // Vänster
                    vector = new Vector2(-texture.Width, r.Next(0, window.ClientBounds.Height - texture.Height));
                    speed = new Vector2(3f, 0);
                    break;
            }

        }

        public List<EnemyBullet> Bullets { get { return bullets; } }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            Vector2 ShooterCenter = new Vector2(vector.X + texture.Width / 2, vector.Y + texture.Height / 2);
            Vector2 PlayerCenter = new Vector2(player.X, player.Y);
            Vector2 direction = PlayerCenter - ShooterCenter;

            //Rotation
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            rotation += MathHelper.PiOver2;

            switch (sida)
            {
                case 0: // Uppifrån
                    if (vector.Y < targetPos) vector.Y += speed.Y;
                    break;
                case 1: // Höger
                    if (vector.X > window.ClientBounds.Width - texture.Width - targetPos) vector.X -= speed.X;
                    break;
                case 2: // Nedifrån
                    if (vector.Y > window.ClientBounds.Height - texture.Height - targetPos) vector.Y -= speed.Y;
                    break;
                case 3: // Vänster
                    if (vector.X < targetPos) vector.X += speed.X;
                    break;
            }


            //Kontrollera när shooter ska skjuta
            if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 1000)
            {
                if (direction != Vector2.Zero)
                    direction.Normalize();

                float bulletSpeed = 10f;
                Vector2 velocity = direction * bulletSpeed;

                EnemyBullet temp = new EnemyBullet(bulletTexture, ShooterCenter.X, ShooterCenter.Y, velocity.X, velocity.Y);
                bullets.Add(temp);
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Flytta på alla skott
            foreach (EnemyBullet b in bullets.ToList())
            {
                //Flytta på skottet
                b.Update();
                //Kontrollera att skottet inte är dött
                if (!b.IsAlive)
                    bullets.Remove(b);

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 rotationspunkt = new Vector2(texture.Width/2, texture.Height/2);

            spriteBatch.Draw(texture, new Vector2(vector.X + texture.Width, vector.Y + texture.Height / 2), null, Color.White, rotation, rotationspunkt, 1.0f, SpriteEffects.None, 0f);

            //Rita skotten
            foreach (EnemyBullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }


        public void Reset(float X, float Y, float speedX, float speedY)
        {
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            //Återställ spelarens position och hastighet 
            bullets.Clear();
            timeSinceLastBullet = 0;

            //Gör så att spelaren lever igen
            isAlive = true;
        }
    }
}
