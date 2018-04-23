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
    class Machinegun3 : Machinegun2
    {
        public Machinegun3(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 2;
            shotTimer = new Timer(0.2f);
            shotDrawTimer = new Timer(0.2f);
            range = 120;
            upgradePrice = 250;
            canHitFlying = true;
            canHitFlying = true;
        }
    }
}
