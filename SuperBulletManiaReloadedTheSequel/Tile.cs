using Microsoft.Xna.Framework;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    class Tile : Entity
    {
        public Tile(DrawerCollection textures_, Vector2 pos_, List<Property> properties_) : base(textures_, pos_, properties_, "tile", "tile") { }

    }
}
