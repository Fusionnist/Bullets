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
    public class EnemyFat : Enemy
    {
        //next pt in map
        Vector2 point;
        Timer deathTimer;
        Random r;
        bool looted;

        public EnemyFat(DrawerCollection t_, Vector2 p_, List<Property> prop_, string name_) : base(t_, p_, prop_, name_)
        {
            point = pos;
            deathTimer = new Timer(1);
            hp = 10;
            r = new Random();
            speed = 0.2f;
            worth = 2;
        }

        public override void Update(float elapsedTime_)
        {
            base.Update(elapsedTime_);
        }
    }
}
