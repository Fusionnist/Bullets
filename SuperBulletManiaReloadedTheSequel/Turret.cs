using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using MonoGame.FZT.UI;
using System;
using System.Collections.Generic;
using static SuperBulletManiaReloadedTheSequel.Enums;

namespace SuperBulletManiaReloadedTheSequel
{
    class Turret : Entity
    {
        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_): base(texes_, pos_, properties_, "turret", "turret")
        { }

        public override void Update(float elapsedTime_)
        {
            base.Update(elapsedTime_);
        }
    }
}
