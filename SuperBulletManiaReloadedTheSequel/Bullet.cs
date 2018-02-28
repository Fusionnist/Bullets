using Microsoft.Xna.Framework;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Drawing;
using System;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{
    class Bullet : Entity
    {
        float angle;

        public Bullet(DrawerCollection texes_, Vector2 pos_, List<Property> props_, float angle_): base(texes_, pos_, props_, "bullet", "bullet")
        {
            angle = angle_;
            vel = new Vector2((float)Math.Cos(angle) * 500, (float)Math.Sin(angle) * 500);
        }

        public void Bounce(bool isVertical_)
        {
            if (isVertical_)
                angle = (float)Math.PI - angle;
            else
                angle = -angle;
            vel = new Vector2((float)Math.Cos(angle) * 500, (float)Math.Sin(angle) * 500);
        }

        public Bullet CloneForShoot(float angle_, Vector2 pos_)
        {
            return new Bullet(textures, pos_, properties, angle_);
        }
    }
}
