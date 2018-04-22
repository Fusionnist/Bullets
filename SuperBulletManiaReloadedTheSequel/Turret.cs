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
        protected int baseDmg;
        Vector2 target;
        float angle;
        bool isShooting;
        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret"): base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
        }

        public override void Update(float elapsedTime_)
        {
            //damage first enemy
            isShooting = false;
            if (EntityCollection.GetGroup("enemies").Count > 0)
            {
                Entity tar = null;
                foreach (Entity e in EntityCollection.GetGroup("enemies"))
                {
                    if (!e.isDestroyed)
                    {
                        tar = e;
                        break;
                    }
                }
                if(tar != null)
                {
                    isShooting = true;
                    tar.TakeDamage(baseDmg);
                    target = tar.pos;

                    angle = -(float)Math.Atan2(target.X - pos.X, target.Y - pos.Y) + (float)Math.PI;
                    //angle -= angle%((float)Math.PI / 16);
                }                
            }
            base.Update(elapsedTime_);
        }

        public override void Draw(SpriteBatch sb_, bool flipH_ = false, float angle_ = 0f)
        {
            if (isShooting) { currentTex = textures.GetTex("t1shoot"); }
            else { currentTex = textures.GetTex("t1idle"); }
            base.Draw(sb_, flipH_, angle);
        }
    }
}
