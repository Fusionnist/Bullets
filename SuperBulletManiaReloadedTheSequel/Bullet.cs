using Microsoft.Xna.Framework;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    class Bullet : Entity
    {
        public Bullet(DrawerCollection texes_, Vector2 pos_, List<Property> props_): base(texes_, pos_, props_, "bullet", "bullet")
        {

        }


    }
}
