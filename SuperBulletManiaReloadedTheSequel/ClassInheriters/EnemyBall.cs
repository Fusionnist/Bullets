using Microsoft.Xna.Framework;
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
    public class EnemyBall : Enemy
    {
        //next pt in map
        Vector2 point;
        Timer deathTimer;
        Random r;
        bool looted;

        public EnemyBall(DrawerCollection t_, Vector2 p_, List<Property> prop_, string name_) : base(t_, p_, prop_, name_)
        {
            point = pos;
            deathTimer = new Timer(1);
            hp = 80;
            r = new Random();
            speed = 0.5f;
            worth = 1;
        }

        public override void Update(float elapsedTime_)
        {
            base.Update(elapsedTime_);
        }
    }
}
