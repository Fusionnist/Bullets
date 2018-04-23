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
    class Machinegun1 : Turret
    {
        public Machinegun1(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 1;
            shotTimer = new Timer(0.8f);
            shotDrawTimer = new Timer(0.2f);
            range = 80;
            upgradePrice = 10;
        }

        protected override List<Entity> ObtainTargets()
        {
            List<Entity> ents = new List<Entity>();
            if (EntityCollection.GetGroup("enemies").Count > 0)
            {
                Entity tar = null;
                foreach (Entity e in EntityCollection.GetGroup("enemies"))
                {
                    if (!e.isDestroyed)
                    {
                        if ((e.pos - pos).Length() <= range)
                        {
                            if (!e.BoolProperty("flying") || canHitFlying)
                            {
                                tar = e;
                                break;
                            }
                        }
                    }
                }
                ents.Add(tar);
            }

            return ents;
        }
    }
}
