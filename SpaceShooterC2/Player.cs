using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Player : PhysicalObject
    {
        //Konstruktor
        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture) : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        //Egenskaper
        int points = 0;
        public int Points 
        { 
            get { return points; } 
            set { points = value; } 
        }
  
        public List<Bullet> Bullets { get { return bullets; } }

        bool isInvincible;
        public bool IsInvincible
        {
            get { return isInvincible; }
            set { isInvincible = value; }
        }

        public double InvincibleUntil
        {
            get { return InvincibleUntil; }
            set { InvincibleUntil = value; }
        }

        public double Time;

        float rotation = 0f;


        //Powerups
        public bool harRapidfire
        {
            get { return harRapidfire; }
            set { harRapidfire = value; }
        }



        List<Bullet> bullets; //Allla skott 
        Texture2D bulletTexture; //skottets bild
        double timeSinceLastBullet = 0; //millesekunder

        public void Update(GameWindow gameWindow, GameTime gameTime)
        {
            //Mus och spelar position
            MouseState mouse = Mouse.GetState();
            Vector2 playerCenter = new Vector2(vector.X + texture.Width / 2, vector.Y + texture.Height / 2);
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            Vector2 direction = mousePosition - playerCenter;

            //Rotation
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            rotation += MathHelper.PiOver2;

            Movement(gameWindow);
            Skjutfunktion(gameTime, playerCenter, direction);
            
            //När man tar skada 
            Time = gameTime.TotalGameTime.TotalMilliseconds;
            if(IsInvincible && Time > InvincibleUntil)
                IsInvincible = false;

            // Escape
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                isAlive = false;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            bool blink = false;

            //När spelaren är osårbar
            if (IsInvincible)
            {
                if((int)(Time / 150) % 2 == 0)
                    blink = true;

            }

            //Rita spelaren
            if (!blink)
            {
                Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);

                //Mitten av player
                Vector2 drawPosition = new Vector2(vector.X + texture.Width/2, vector.Y + texture.Height/2);

                spriteBatch.Draw(texture, drawPosition, null, Color.White, rotation,origin, 1.0f,SpriteEffects.None, 0f);
            }

            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
        }


        private void Movement(GameWindow gameWindow)
        {
            //Movement
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                    vector.X += speed.X * 2;
                else
                    vector.X += speed.X;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                    vector.X -= speed.X * 2;
                else
                    vector.X -= speed.X;
            }


            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                    vector.Y += speed.Y * 2;
                else
                    vector.Y += speed.Y;
            }


            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                    vector.Y -= speed.Y * 2;
                else
                    vector.Y -= speed.Y;
            }


            //Bounderies
            if (vector.X < 0)
                vector.X = 0;
            if (vector.X > gameWindow.ClientBounds.Width - texture.Width)
                vector.X = gameWindow.ClientBounds.Width - texture.Width;

            if (vector.Y < 0)
                vector.Y = 0;
            if (vector.Y > gameWindow.ClientBounds.Height - texture.Height)
                vector.Y = gameWindow.ClientBounds.Height - texture.Height;

        }

        private void Skjutfunktion(GameTime gameTime, Vector2 playerCenter, Vector2 direction)
        {
            //Skjutfunktion
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //Kontrollera ifall spelaren får skjuta
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    if (direction != Vector2.Zero)
                        direction.Normalize();

                    float bulletSpeed = 10f;


                    //Rapidfire skott
                    if (harRapidfire)
                    {
                        float spreadVinkel = 0.26f;

                        for (int i = -1; i <= 1; i++)
                        {
                            float CurrentRotation = (float)Math.Atan2(direction.Y, direction.X) + (i * spreadVinkel);
                            Vector2 nyDirection = new Vector2((float)Math.Cos(CurrentRotation), (float)Math.Sin(CurrentRotation));

                            Vector2 velocity = nyDirection * bulletSpeed;
                            Bullet temp = new Bullet(bulletTexture, playerCenter.X, playerCenter.Y, velocity.X, velocity.Y);
                            bullets.Add(temp);
                        }
                    }
                    //Vanligt skott
                    else
                    {
                        Vector2 velocity = direction * bulletSpeed;

                        Bullet temp = new Bullet(bulletTexture, playerCenter.X, playerCenter.Y, velocity.X, velocity.Y);
                        bullets.Add(temp);
                    }
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }


            //Flytta på alla skott
            foreach (Bullet b in bullets.ToList())
            {
                //Flytta på skottet
                b.Update();
                //Kontrollera att skottet inte är dött
                if (!b.IsAlive)
                    bullets.Remove(b);

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
            //Återställ spelarens poäng
            points = 0;

            //Gör så att spelaren lever igen
            isAlive = true; 
        }
    }
}
