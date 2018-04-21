using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using System;
using System.Collections.Generic;
namespace SuperBulletManiaReloadedTheSequel
{
    public class TDEntityBuilder : EntityBuilder
    {
        public TDEntityBuilder()
        {

        }

        public override Entity CreateEntity(string type_, DrawerCollection dc_, Vector2 pos_, List<Property> props_, string name_)
        {
            if (type_ == "turret")
            {
                return new Turret(dc_, pos_, props_);
            }
            return base.CreateEntity(type_, dc_, pos_, props_, name_);
        }
    }
}
