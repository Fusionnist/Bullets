﻿using Microsoft.Xna.Framework;
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
    class Machinegun2 : Machinegun1
    {
        public Machinegun2(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
            shotTimer = new Timer(0.5f);
            shotDrawTimer = new Timer(0.2f);
            range = 100;
            upgradePrice = 50;
        }
    }
}
