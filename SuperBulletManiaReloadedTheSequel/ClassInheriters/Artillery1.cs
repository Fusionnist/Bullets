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
    class Artillery1 : Turret
    {
        protected int splodeRange;
        public Artillery1(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 3;
            shotTimer = new Timer(2f);
            shotDrawTimer = new Timer(0.2f);
            range = 80;
            upgradePrice = 15;
            
            splodeRange = 15;
        }

        protected override List<Entity> ObtainTargets()
        {
            Vector2 firstPos = new Vector2(0, 0);
            List<Entity> ents = new List<Entity>();
            if (EntityCollection.GetGroup("enemies").Count > 0)
            {
                foreach (Entity e in EntityCollection.GetGroup("enemies"))
                {
                    if (!e.isDestroyed)
                    {
                        if ((e.pos - pos).Length() <= range)
                        {
                            if (!e.BoolProperty("flying") || canHitFlying)
                            {
                                firstPos = e.pos;
                                break;
                            }
                        }
                    }
                }
                foreach (Entity e in EntityCollection.GetGroup("enemies"))
                {
                    if (!e.isDestroyed)
                    {
                        if ((e.pos - pos).Length() <= range)
                        {
                            if (!e.BoolProperty("flying") || canHitFlying)
                            {
                                if((e.pos-firstPos).Length() <= splodeRange)
                                {
                                    ents.Add(e);
                                }
                            }
                        }
                    }
                }
            }
            return ents;
        }
    }
}
