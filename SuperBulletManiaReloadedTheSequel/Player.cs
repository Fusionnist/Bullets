using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using Microsoft.Xna.Framework;

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
