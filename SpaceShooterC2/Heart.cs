using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooterC2
{
    internal class Heart: Item
    {
        public Heart(Texture2D texture, float X, float Y, GameTime gameTime) : base(X, Y, gameTime)
        { }
    }
}
