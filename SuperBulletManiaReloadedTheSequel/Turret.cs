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
        Vector2 target;
        float angle;
        protected bool isShooting, canHitFlying;
        protected Timer shotTimer, shotDrawTimer;

        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret"): base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
            shotTimer = new Timer(0.5f);
            shotDrawTimer = new Timer(0.2f);
            range = 100;
            upgradePrice = 50;
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

                    angle = -(float)Math.Atan2(target.X - pos.X, target.Y - pos.Y) + (float)Math.PI;
                    angle -= angle%((float)Math.PI / 8); 
                }
            }
            if (wasTarget)
            {
                shotDrawTimer.Reset();

                SoundManager.PlayEffect("shoot");
            }
        }

        public override void Draw(SpriteBatch sb_, bool flipH_ = false, float angle_ = 0f)
        {
            if (isShooting) { currentTex = textures.GetTex("t1shoot"); }
            else if(currentTex.Ended()) { currentTex = textures.GetTex("t1idle"); }
            base.Draw(sb_, flipH_, angle);
        }
    }
}
