using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Sound;
using MonoGame.FZT.UI;
using System;
using System.Collections.Generic;
using static SuperBulletManiaReloadedTheSequel.Enums;

namespace SuperBulletManiaReloadedTheSequel
{
    class Turret : Entity
    {
        protected int baseDmg, range;
        Vector2 target;
        float angle;
        bool isShooting;
        protected Timer shotTimer, shotDrawTimer;

        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret"): base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
            shotTimer = new Timer(0.5f);
            shotDrawTimer = new Timer(0.2f);
            range = 100;
        }

        public override void Update(float elapsedTime_)
        {
            isShooting = false;
            shotTimer.Update(elapsedTime_);
            shotDrawTimer.Update(elapsedTime_);
            if (shotTimer.Complete())
            {
                shotTimer.Reset();
                Shoot();
            }
            
            base.Update(elapsedTime_);
        }

        void Shoot()
        {
            //damage first enemy
            if (EntityCollection.GetGroup("enemies").Count > 0)
            {
                Entity tar = null;
                foreach (Entity e in EntityCollection.GetGroup("enemies"))
                {
                    if (!e.isDestroyed)
                    {
                        if((e.pos - pos).Length() <= range)
                        {
                            tar = e;
                            break;
                        }                        
                    }
                }
                if (tar != null)
                {
                    isShooting = true;
                    tar.TakeDamage(baseDmg);
                    target = tar.pos;

                    angle = -(float)Math.Atan2(target.X - pos.X, target.Y - pos.Y) + (float)Math.PI;
                    //angle -= angle%((float)Math.PI / 16);

                    shotDrawTimer.Reset();

                    SoundManager.PlayEffect("shoot");
                }
            }
        }

        public override void Draw(SpriteBatch sb_, bool flipH_ = false, float angle_ = 0f)
        {
            if (isShooting) { currentTex = textures.GetTex("t1shoot"); }
            else if(shotDrawTimer.Complete()) { currentTex = textures.GetTex("t1idle"); }
            base.Draw(sb_, flipH_, angle);
        }
    }
}
