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
        public List<Bullet> Bullets { get { return bullets; } }
        List<Bullet> bullets;
        double timeSinceLastBullet = 0;

        public Shooter(Texture2D texture, float X, float Y, Player player, Texture2D bulletTexture) : base(texture, X, Y, 6f, 0.3f)
        {
            this.player = player;
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        public override void Update(GameWindow window, GameTime gameTime)
        {
            Vector2 ShooterCenter = new Vector2(vector.X + texture.Width / 2, vector.Y + texture.Height / 2);
            Vector2 PlayerCenter = new Vector2(player.PlayerPosX, player.PlayerPosY);
            Vector2 direction = PlayerCenter - ShooterCenter;

            if (vector.Y > 100)
            {
                vector.Y++;
            }

                    //Kontrollera ifall spelaren får skjuta
                    if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                    {
                        if (direction != Vector2.Zero)
                            direction.Normalize();

                        float bulletSpeed = 10f;
                        Vector2 velocity = direction * bulletSpeed;

                        Bullet temp = new Bullet(bulletTexture, ShooterCenter.X, ShooterCenter.Y, velocity.X, velocity.Y); 
                        bullets.Add(temp);
                        timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                
            }
            
        }


    }
}
