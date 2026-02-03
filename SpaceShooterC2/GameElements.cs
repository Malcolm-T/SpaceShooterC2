using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.IO;

//using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace SpaceShooterC2
{


    static class GameElements
    {
        static Player player; 
        public static Player Player { get { return player; } } //Gör synlig överallt
        static Texture2D background;
        static Texture2D Title;

        static List<Enemy> enemies;

        //Coins
        static List<Coin> coins; 
        static Texture2D coinSprite;

        //Hearts
        static List<Heart> hearts;
        static Texture2D heartsprite;

        //Rapidfire
        static List<Rapidfire> rapidFires;
        static Texture2D rapidFireSprite;
        static double rapidTimer;

        static PrintText printText;

        //Level
        static int level = 1;
        static int enemiesKilled = 0;

        //Liv
        static int liv = 3;
        static double coolDown;
        static bool skadlig = true;

        static bool sparatScore = false;


        //Olika gamestates
        public enum State { Menu, Run, Highscore, Quit};
        public static State currentState;
        static Menu menu;
        static Highscore highscore;

        public static void Initialize()
        {
            coins = new List<Coin>();
            hearts = new List<Heart>();
            rapidFires = new List<Rapidfire>();
        }

        public static void LoadContent(ContentManager content, GameWindow window)
        {
            //Menu
            menu = new Menu((int)State.Menu);
            menu.AddItem(content.Load<Texture2D>("images/menu/start"), (int)State.Run);
            menu.AddItem(content.Load<Texture2D>("images/menu/highscore"), (int)State.Highscore);
            menu.AddItem(content.Load<Texture2D>("images/menu/exit"), (int)State.Quit);

            background = content.Load<Texture2D>("Kroppen/Bakgrund");

            player = new Player(content.Load<Texture2D>("images/player/ship"), 380, 400, 2.5f, 4.5f, content.Load<Texture2D>("images/player/bullet"));

            //Title
            Title = content.Load<Texture2D>("Kroppen/CellWars");


            //Skapa Fiender
            enemies = new List<Enemy>();
            SpawnEnemies(window, content);
            coinSprite = content.Load<Texture2D>("images/powerups/coin"); //Coin sprite
            heartsprite = content.Load<Texture2D>("images/powerups/Hjärta1"); //Heart sprite
            rapidFireSprite = content.Load<Texture2D>("images/powerups/Rapidfire"); //Rapidfire sprite


            printText = new PrintText(content.Load<SpriteFont>("myFont"));
        }

        public static State MenuUpdate(GameTime gameTime)
        {

            return (State)menu.Update(gameTime);
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
            spriteBatch.Draw(Title, new Vector2(105,20), null, Color.White, 0f, Vector2.Zero,0.3f, SpriteEffects.None, 0f);
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
                        b.IsAlive = false;
                        player.Points++;
                        enemiesKilled++;
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
                            
                        }
                    }
                    enemy.Update(window, gameTime);
                }
                else
                    enemies.Remove(enemy);
            }

            //Hit-reg. bullets på player
            foreach(Enemy enemy in enemies)
            {
                if(enemy is Shooter shooter)
                {
                    foreach (EnemyBullet b in shooter.Bullets)
                    {
                        if (skadlig)
                        {
                            if (Player.CheckCollision(b))
                            {
                                b.IsAlive = false;
                                liv--;
                                coolDown = gameTime.TotalGameTime.TotalMilliseconds + 3000;
                            }
                        }
                    }
                }
            }


            //Coins
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

            //Hearts
            int newHeart = random.Next(1, 400);
            if (newHeart == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - heartsprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - heartsprite.Height);

                hearts.Add(new Heart(heartsprite, rndX, rndY, gameTime));
            }

            foreach (Heart h in hearts.ToList())
            {
                if (h.IsAlive)
                {
                    h.Update(gameTime);
                     
                    if (h.CheckCollision(player))
                    {
                        if(liv < 3)
                        {
                            hearts.Remove(h); 
                            liv++;
                        }
                        else
                            hearts.Remove(h);
                    }
                }
                else
                    hearts.Remove(h);
            }

            //Rapidfire
            int newRapidFire = random.Next(1, 1000);
            if (newRapidFire == 1)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - rapidFireSprite.Width);
                int rndY = random.Next(0, window.ClientBounds.Height - rapidFireSprite.Height);

                rapidFires.Add(new Rapidfire(rapidFireSprite, rndX, rndY, gameTime));
            }



            foreach (Rapidfire r in rapidFires.ToList())
            {
                if (r.IsAlive)
                {
                    r.Update(gameTime);

                    if (r.CheckCollision(player))
                    {
                        player.harRapidfire = true;
                        r.IsAlive = false;
                        rapidTimer = gameTime.TotalGameTime.TotalMilliseconds + 5000;
                    }
                }
                else
                    rapidFires.Remove(r);
            }
            if (rapidTimer < gameTime.TotalGameTime.TotalMilliseconds)
                player.harRapidfire = false;


            //Checkar om level upp
            if (enemies.Count == 0)
            {
                level++;
                enemiesKilled = 0;
                SpawnEnemies(window, content);
            }


            if (!player.IsAlive)
            {
                Reset(window, content);
                return State.Menu;
            }


            if (liv <= 0)
            {
                if (!sparatScore)
                {
                    using (StreamWriter writer = new StreamWriter("highscore.txt", true))
                    {
                        writer.WriteLine(player.Points + "\t" + DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    highscore = new Highscore();
                    sparatScore = true;
                }



                player.IsAlive = false;
                return State.Highscore;
            }
            return State.Run;

        }


        public static void RunDraw(SpriteBatch spriteBatch, GameWindow window, GameTime gameTime)
        {
            //Bakgrund
            spriteBatch.Draw(background, new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height), Color.White);

            //Rita ut spelare och fiender
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach(Coin c in coins.ToList())
                c.Draw(spriteBatch);
            foreach (Heart h in hearts.ToList())
                h.Draw(spriteBatch);
            foreach (Rapidfire r in rapidFires.ToList())
                r.Draw(spriteBatch);

            printText.Print("Points " + player.Points, spriteBatch, 0, 0);

            //Liv
            int heartsX = window.ClientBounds.Width - 50;
            int heartsY = 10;
            int spacing = heartsprite.Width + 10;
            for(int i = 0; i < liv; i++)
            {
                spriteBatch.Draw(heartsprite, new Vector2(heartsX - i * spacing, heartsY), Color.White);
            }


            printText.Print("Level: " + level, spriteBatch, 0, 30); 

        }

        public static State HighScoreUpdate()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //Återgå till meny om man klickar esc
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                return State.Menu;
            }
            return State.Highscore;
        }

        public static void HighscoreDraw(SpriteBatch spriteBatch)
        {
            //Rita ut highscore-listan 
            printText.Print("HIGHSCORE", spriteBatch, 300, 70);
            printText.Print(Player.Points.ToString() + " points", spriteBatch, 300, 30);

            int y = 120;
            int plats = 1;

            //Utskrift av topplista
            if(highscore.Scores.Count > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    printText.Print(plats + ". " + highscore.Scores[i].Poäng + "   " + highscore.Scores[i].Datum, spriteBatch, 250, y);
                    y += 30;
                    plats++;
                }
            }
            else
            {
                for (int i = 0; i < highscore.Scores.Count; i++)
                {
                    printText.Print(plats + ". " + highscore.Scores[i].Poäng + "   " + highscore.Scores[i].Datum, spriteBatch, 250, y);
                    y += 30;
                    plats++;
                }
            }

            printText.Print("Tryck space", spriteBatch, 300, 300);


        }

        private static void Reset(GameWindow window, ContentManager content)
        {
            player.Reset(380, 400, 2.5f, 4.5f);

            level = 1;
            SpawnEnemies(window, content);

            liv = 3; 
        }


        static void SpawnEnemies(GameWindow window, ContentManager content)
        {
            enemies.Clear();
            Random random = new Random();

            //Antal fiender baserat på level
            int mines = 3 + level;
            int tripods = 2 + level/2;
            int shooters = level>=3 ? level-2 : 0;

            //Sprites
            Texture2D mineSprite = content.Load<Texture2D>("Kroppen/Virus1");
            Texture2D tripodSprite = content.Load<Texture2D>("Kroppen/Virus2");
            Texture2D shooterSprite = content.Load<Texture2D>("Kroppen/Shooter2");


            //Skapa fiender
            for (int i = 0; i < mines; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - mineSprite.Width);
                int rndY = random.Next(- 50, 0);
                Mine temp = new Mine(mineSprite, rndX, rndY, player);
                enemies.Add(temp); //Lägg till i listan
            }

            for (int i = 0; i < tripods; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - tripodSprite.Width);
                int rndY = random.Next(- 50, 0);
                Tripod temp = new Tripod(tripodSprite, rndX, rndY);
                enemies.Add(temp);
            }

            for(int i = 0; i < shooters; i++)
            {
                int rndX = random.Next(0, window.ClientBounds.Width - shooterSprite.Width);
                int rndY = random.Next(-50,0);
                Shooter temp = new Shooter(shooterSprite, rndX, rndY, player, content.Load<Texture2D>("images/player/Enemies/evilBullet"));
                enemies.Add(temp);
            }

        }


    }
}
