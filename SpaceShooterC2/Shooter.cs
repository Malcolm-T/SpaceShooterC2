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
        //Egenskaper
        private Player player;
        Texture2D bulletTexture;
        List<EnemyBullet> bullets;
        double timeSinceLastBullet = 0;

        public Shooter(Texture2D texture, float X, float Y, Player player, Texture2D bulletTexture) : base(texture, X, Y, 6f, 0.3f)
        {
            this.player = player;
            bullets = new List<EnemyBullet>();
            this.bulletTexture = bulletTexture;
        }

        public List<EnemyBullet> Bullets { get { return bullets; } }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            Vector2 ShooterCenter = new Vector2(vector.X + texture.Width / 2, vector.Y + texture.Height / 2);
            Vector2 PlayerCenter = new Vector2(player.PlayerPosX, player.PlayerPosY);
            Vector2 direction = PlayerCenter - ShooterCenter;

            if (vector.Y < 100)
            {
                vector.Y++;
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
            base.Draw(spriteBatch);
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
