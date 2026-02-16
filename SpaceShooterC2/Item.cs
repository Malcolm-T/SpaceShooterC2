using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    public enum ItemType
    {
        Coin,
        Heart,
        Rapidfire
    }

    internal class Item : PhysicalObject
    {
        public ItemType Type { get; private set; }

        double timeToDie;
        public Item(Texture2D texture, float X, float Y, ItemType type, GameTime gameTime) : base(null, X, Y, 0, 2f)
        {
            timeToDie = gameTime.TotalGameTime.TotalMilliseconds + 5000;
            this.Type = type;
        }

        public void Update(GameTime gameTime)
        {
            if (timeToDie < gameTime.TotalGameTime.TotalMilliseconds)
            {
                isAlive = false;
            }
        }
    }
}
