using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Menu
    {
        List<MenuItem> menu; //Lista på menu items
        int selected = 0; //Färsta valet i listan är valt

        //currentHeigh används för att rita ut menuItems på olika höjd
        float currentHeight = 150;

        //Lastchange används för att "pausa" tangentbordtryckningar
        double lastChange = 0;
        int defaultMenuState;


        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }

        public void AddItem(Texture2D itemTexture, int state)
        {
            float X = 200; 
            float Y = 0 + currentHeight;

            currentHeight += itemTexture.Height + 20;

            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);
            menu.Add(temp);

        }

        public int Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    selected++;
                    if (selected > menu.Count - 1)
                        selected = 0;
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    selected--;
                    if (selected < 0)
                        selected = menu.Count - 1;
                }

                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                return menu[selected].State;
            }
            
            return defaultMenuState;

        }            

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i<menu.Count; i++)
            {
                if (i == selected)
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].Position.X, menu[i].Position.Y), Color.RosyBrown);
                else
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
            }
        }

        class MenuItem
        {
            Texture2D texture;
            Vector2 position;
            int currentState;

            public MenuItem(Texture2D texture, Vector2 position, int currentState)
            {
                this.texture = texture;
                this.position = position;
                this.currentState = currentState;
            }

            public Texture2D Texture { get { return texture; } }
            public Vector2 Position { get { return position; } }
            public int State { get { return currentState; } }
        }
    }
}
