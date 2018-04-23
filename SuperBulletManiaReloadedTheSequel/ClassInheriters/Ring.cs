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
    class Ring : Turret
    {
        protected int splodeRange;
        public Ring(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 50;
            shotTimer = new Timer(5f);
            shotDrawTimer = new Timer(0.2f);
            range = 80;
            upgradePrice = 15;

            splodeRange = 50;
        }

        protected override List<Entity> ObtainTargets()
        {
            List<Entity> ents = new List<Entity>();
            foreach (Entity e in EntityCollection.GetGroup("enemies"))
            {
                if (!e.isDestroyed)
                {
                    if (!e.BoolProperty("flying") || canHitFlying)
                    {
                        if ((e.pos - pos).Length() <= splodeRange)
                        {
                            ents.Add(e);
                        }
                    }
                }
            }
            return ents;
        }

        protected override void DamageTargets(List<Entity> targets)
        {
            base.DamageTargets(targets);
            angle = 0;
        }
    }
}
