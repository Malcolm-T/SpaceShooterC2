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
        public List<Bullet> Bullets { get { return bullets; } }

        public bool IsInvincible { get; set; }
        public double InvincibleUntil { get; set; }
        public double Time { get; set; }

        int points = 0;

        public float PlayerPosX { get { return vector.X; } }
        public float PlayerPosY { get { return vector.Y; } }

        public float MousePosX { get { return Mouse.GetState().X; } }
        public float MousePosY { get { return Mouse.GetState().Y; } }


        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture) : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        public int Points { get { return points; } set { points = value; } }
        List<Bullet> bullets; //Allla skott 
        Texture2D bulletTexture; //skottets bild
        double timeSinceLastBullet = 0; //millesekunder

        public void Update(GameWindow gameWindow, GameTime gameTime)
        {
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
                    vector.Y += speed.Y*2;
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



            if (vector.X < 0)
                vector.X = 0;
            if(vector.X > gameWindow.ClientBounds.Width - texture.Width)
                vector.X = gameWindow.ClientBounds.Width - texture.Width;

            if (vector.Y < 0)
                vector.Y = 0;
            if (vector.Y > gameWindow.ClientBounds.Height - texture.Height)
                vector.Y = gameWindow.ClientBounds.Height - texture.Height;


            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //Kontrollera ifall spelaren får skjuta
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    Vector2 playerPosition = new Vector2(vector.X + texture.Width / 2, vector.Y + texture.Height / 2);

                    MouseState mouse = Mouse.GetState();
                    Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);

                    Vector2 direction = mousePosition - playerPosition;

                    if(direction != Vector2.Zero)
                        direction.Normalize();

                    float bulletSpeed = 10f;
                    Vector2 velocity = direction * bulletSpeed;

                    Bullet temp = new Bullet(bulletTexture, playerPosition.X, playerPosition.Y, velocity.X, velocity.Y);
                    bullets.Add(temp); 
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;


                    ////Skapa skottet
                    //Bullet temp = new Bullet(bulletTexture, vector.X + texture.Width / 2, vector.Y, );
                    //bullets.Add(temp); //Lägger till skottet i listan

                    ////Sätt timeSinceLastBullet till detta ögonblick 
                    //timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
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


            if (keyboardState.IsKeyDown(Keys.Escape))
                isAlive = false;

            Time = gameTime.TotalGameTime.TotalMilliseconds;
            if(IsInvincible && Time > InvincibleUntil)
                IsInvincible = false;
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
                spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
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
