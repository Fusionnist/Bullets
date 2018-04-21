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
        int baseDmg;
        Vector2 target;
        float angle;
        public Turret(DrawerCollection texes_, Vector2 pos_, List<Property> properties_): base(texes_, pos_, properties_, "turret", "turret")
        {
            baseDmg = 10;
        }

        public override void Update(float elapsedTime_)
        {
            //damage first enemy
            if (EntityCollection.GetGroup("enemies").Count > 0)
            {
                EntityCollection.GetGroup("enemies")[0].TakeDamage(10);
                target = EntityCollection.GetGroup("enemies")[0].pos;
            }
            angle += 0.01f;
            base.Update(elapsedTime_);
        }

        public override void Draw(SpriteBatch sb_, bool flipH_ = false, float angle_ = 0f)
        {
            base.Draw(sb_, flipH_, angle);
        }
    }
}
