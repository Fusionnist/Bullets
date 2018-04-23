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
        protected int baseDmg, range, upgradePrice;
        protected Vector2 target;
        protected float angle;
        protected bool isShooting, canHitFlying;
        protected Timer shotTimer, shotDrawTimer;
        Random r;

        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret"): base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
            shotTimer = new Timer(0.5f);
            shotDrawTimer = new Timer(0.2f);
            range = 100;
            upgradePrice = 50;
            r = new Random();
        }

        public override int GetValue(string context_)
        {
            if(context_ == "upgradePrice") { return upgradePrice; }
            return base.GetValue(context_);
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
            if(textures.GetTex("t1shoot").Ended())
                textures.GetTex("t1shoot").Reset();
            DamageTargets(ObtainTargets());
        }

        protected virtual List<Entity> ObtainTargets()
        {
            return new List<Entity>();
        }

        protected virtual void DamageTargets(List<Entity> targets)
        {
            bool wasTarget = false;
            foreach(Entity tar in targets)
            {
                if (tar != null)
                {
                    isShooting = true;
                    tar.TakeDamage(baseDmg);
                    target = tar.pos;
                    wasTarget = true;

                    angle = -(float)Math.Atan2(target.X - pos.X, target.Y - pos.Y) + (float)Math.PI;
                    //if (angle % ((float)Math.PI / 8) < ((float)Math.PI / 8)/2)
                        angle -= angle%((float)Math.PI / 8);
                    //else
                    //{
                    //    angle += angle % ((float)Math.PI / 8);
                    //}
                }
            }
            if (wasTarget)
            {
                shotDrawTimer.Reset();

                if(r.Next(0,100)>80)
                SoundManager.PlayEffect("shoot");
            }
        }

        public override void Draw(SpriteBatch sb_, bool flipH_ = false, float angle_ = 0f)
        {
            if (isShooting)
            { currentTex = textures.GetTex("t1shoot"); textures.GetTex("shot").Reset();  }
            else if(currentTex.Ended() && currentTex.name != "t1idle")
            { currentTex = textures.GetTex("t1idle"); }
            else if(!textures.GetTex("shot").Ended())
            {
                textures.GetTex("shot").Draw(sb_, target);
            }

            currentTex.Draw(sb_, pos + new Vector2(0, 2), rotation_:angle);
            currentTex.Draw(sb_, pos + new Vector2(0, 1), rotation_: angle);
            base.Draw(sb_, flipH_, angle);
        }
    }
}
