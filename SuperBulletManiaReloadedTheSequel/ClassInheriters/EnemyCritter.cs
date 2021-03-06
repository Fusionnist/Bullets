﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using System;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    public class EnemyCritter : Enemy
    {
        //next pt in map
        Vector2 point;
        Timer deathTimer;
        Random r;
        bool looted;

        public EnemyCritter(DrawerCollection t_, Vector2 p_, List<Property> prop_, string name_) : base(t_, p_, prop_, name_)
        {
            point = pos;
            deathTimer = new Timer(1);
            hp = 5;
            r = new Random();
            speed = 0.5f;
        }
    }
}
