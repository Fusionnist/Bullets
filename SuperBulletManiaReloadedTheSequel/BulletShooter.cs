using Microsoft.Xna.Framework;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using System;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    class BulletShooter : Tile
    {
        float angle, bulletTimer, bulletTime, rotationSpeed;
        Bullet baseBullet;
        public List<Bullet> bullets;

        public BulletShooter(DrawerCollection texes_, Vector2 pos_, List<Property> props_, float baseAngle_, Bullet baseBullet_, float timer_, float rotSpd_): base(texes_, pos_, props_)
        {
            angle = baseAngle_;
            baseBullet = baseBullet_;
            bulletTimer = timer_;
            bulletTime = bulletTimer;
            bullets = new List<Bullet>();
            rotationSpeed = rotSpd_;
        }


        public override void Update(float elapsedTime_)
        {
            bulletTime -= elapsedTime_;
            if (bulletTime <= 0)
            { bullets.Add(baseBullet.CloneForShoot(angle, pos + HandleExtraPosVec())); bulletTime = bulletTimer; }
            angle += elapsedTime_ * rotationSpeed;
            base.Update(elapsedTime_);
        }

        public Vector2 HandleExtraPosVec()
        {
            Vector2 returned;
            if (Math.Abs(Math.Cos(angle)) > Math.Abs(Math.Sin(angle)))
                returned = new Vector2((float)Math.Round(Math.Cos(angle)), (float)Math.Sin(angle));
            else if (Math.Abs(Math.Cos(angle)) < Math.Abs(Math.Sin(angle)))
                returned = new Vector2((float)Math.Cos(angle), (float)Math.Round(Math.Sin(angle)));
            else
                returned = new Vector2((float)Math.Round(Math.Cos(angle)), (float)Math.Round(Math.Sin(angle)));
            return returned;
        }
    }
}
