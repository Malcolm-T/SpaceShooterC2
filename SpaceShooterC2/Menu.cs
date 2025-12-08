using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        float currentHeight = 0;

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

        }

        public int Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < menu.Count)
            {
                if(i == selected) 
                    spriteBatch.Draw(menu[i].Texture, new Vector2(menu[i].Position.X, menu[i].Position.Y), Color.RosyBrown);
                else
            }
        }

        class MenuItem
        {
            Texture2D texture;
            Vector2 position;
            int currentState;

            public MenuItem(Texture2D texture, Vector2 position, int currenState)
            {
                this.texture = texture;
                this.position = position;
                this.currentState = currenState;
            }

            public Texture2D Texture { get { return texture; } }
            public Vector2 Position { get { return position; } }
            public int CurrentState { get { return currentState; } }
        }
    }
}
