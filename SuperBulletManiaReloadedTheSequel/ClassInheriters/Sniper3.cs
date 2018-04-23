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
    class Sniper3 : Sniper2
    {
        public Sniper3(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            shotTimer = new Timer(3f);
            shotDrawTimer = new Timer(0.2f);
            range = 200;
            upgradePrice = 300;
            baseDmg = 40;
        }
    }
}
