using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;

//using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{


    static class GameElements
    {
        static Player player; 
        public static Player Player { get { return player; } } //Gör synlig överallt

        static List<Enemy> enemies;
        static List<Coin> coins; 
        static Texture2D coinSprite;
        static PrintText printText;

        //Liv
        static int liv = 3;
        static double coolDown;
        static bool skadlig = true;


        //Olika gamestates
        public enum State { Menu, Run, Highscore, Quit};
        public static State currentState;
        static Menu menu;

        public static void Initialize()
        {
            coins = new List<Coin>();
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            menu = new Menu((int)State.Menu);
            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)State.Highscore);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)State.Quit);

            player = new Player(content.Load<Texture2D>("images/player/ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("images/player/bullet"));


            //Skapa Fiender
            //Mina
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("images/player/enemies/mine");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndY, player);
                enemies.Add(temp); //Lägg till i listan
            }

            //Tripod
            tmpSprite = content.Load<Texture2D>("images/player/enemies/tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }

            coinSprite = content.Load<Texture2D>("images/powerups/coin");
            printText = new PrintText(content.Load<SpriteFont>("myFont"));
        }

        public static State MenuUpdate(GameTime gameTime)
        {
            return (State)menu.Update(gameTime);
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }

        public static State RunUpdate(ContentManager content, GameWindow window, GameTime gameTime)
        {
            //Uppdatera spelarens position
            player.Update(window, gameTime);

            //Gå igenom alla fiender

            foreach (Enemy enemy in enemies.ToList())
            {
                foreach (Bullet b in player.Bullets)
                {
                    if (enemy.CheckCollision(b))
                    {
                        enemy.IsAlive = false;
                        player.Points++;
                    }
                }
                if (enemy.IsAlive)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds < coolDown)
                    {
                        skadlig = false;
                        player.IsInvincible = true;
                        player.InvincibleUntil = gameTime.TotalGameTime.TotalMilliseconds + 1000;

                    }

                    else skadlig = true;

                    if (skadlig)
                    {
                        if (enemy.CheckCollision(player))
                        {
                            liv--;
                            coolDown = gameTime.TotalGameTime.TotalMilliseconds + 3000;
                            

                            if (liv == 0)
                            {
                                player.IsAlive = false;
                            }
                        }
                    }
                    enemy.Update(window);
                }
                else
                    enemies.Remove(enemy);
            }
            Random random = new Random();
            int newCoin = random.Next(1, 100);
            if (newCoin == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - coinSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - coinSprite.Height);

                coins.Add(new Coin(coinSprite, rndX, rndY, gameTime));
            }

            foreach (Coin c in coins.ToList())
            {
                if (c.IsAlive)
                {
                    c.Update(gameTime);

                    if (c.CheckCollision(player))
                    {
                        coins.Remove(c);
                        player.Points++;
                    }
                }
                else
                    coins.Remove(c);
            }
            if (!player.IsAlive)
            {
                Reset(window, content);
                return State.Menu;
            }
            return State.Run;

        }


        public static void RunDraw(SpriteBatch spriteBatch, GameWindow window, GameTime gameTime)
        {
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach(Coin c in coins.ToList())
                c.Draw(spriteBatch);
            printText.Print("Points" + player.Points, spriteBatch, 0, 0);

            //Liv
            printText.Print("Liv: " + liv, spriteBatch, window.ClientBounds.Width - 50, 0);
            if (gameTime.TotalGameTime.TotalMilliseconds - coolDown < 0)
                printText.Print(Math.Round((gameTime.TotalGameTime.TotalMilliseconds - coolDown)).ToString(), spriteBatch, 100, 0);


        }

        public static State HighScoreUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //Återgå till meny om man klickar esc
            if (keyboardState.IsKeyDown(Keys.Escape))
                return State.Menu;
            return State.Highscore;
        }

        public static void HighscoreDraw(SpriteBatch spriteBatch)
        {
            //Rita ut highscore-listan 
        }

        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);
            
            //Skapa fiender
            enemies.Clear();
            Random random = new Random();
            Texture2D tmpSprite = content.Load<Texture2D>("images/player/Enemies/mine");
            for(int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height/2);
                Mine temp = new Mine(tmpSprite, rndX, rndY, player);
                enemies.Add(temp); //Lägg till i listan
            }

            //Tripod
            tmpSprite = content.Load<Texture2D>("images/player/enemies/tripod");
            for (int i = 0; i < 5; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }

            liv = 3; 
        }

    }
}
