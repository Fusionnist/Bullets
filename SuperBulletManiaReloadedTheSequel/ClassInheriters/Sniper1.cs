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
    class Sniper1 : Turret
    {
        public Sniper1(DrawerCollection texes_, Vector2 pos_, List<Property> properties_, string name_, string type_ = "turret") : base(texes_, pos_, properties_, name_, "turret")
        {
            baseDmg = 10;
            shotTimer = new Timer(5f);
            shotDrawTimer = new Timer(0.2f);
            range = 200;
            upgradePrice = 20;
            
        }

        protected override List<Entity> ObtainTargets()
        {
            int record = 0;
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
                                if(e.hp > record)
                                {
                                    record = e.hp;
                                    tar = e;
                                }                               
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
