using Microsoft.Xna.Framework;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    class Player : Entity
    {
        public Player(DrawerCollection textures_, Vector2 pos_, List<Property> properties_) : base(textures_, pos_, properties_, "player", "player") { }

        public override void Input(Vector2 input_)
        {
            mov += input_ * 20;
        }
    }
}
