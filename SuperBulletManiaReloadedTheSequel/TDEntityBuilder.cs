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
            if (type_ == "machinegun 1")
            {
                return new Machinegun1(dc_, pos_, props_, name_);
            }
            if (type_ == "machinegun 2")
            {
                return new Machinegun2(dc_, pos_, props_, name_);
            }
            if (type_ == "machinegun 3")
            {
                return new Machinegun3(dc_, pos_, props_, name_);
            }

            if (type_ == "sniper 1")
            {
                return new Sniper1(dc_, pos_, props_, name_);
            }
            if (type_ == "sniper 2")
            {
                return new Sniper2(dc_, pos_, props_, name_);
            }
            if (type_ == "sniper 3")
            {
                return new Sniper3(dc_, pos_, props_, name_);
            }

            if (type_ == "artillery 1")
            {
                return new Artillery1(dc_, pos_, props_, name_);
            }
            if (type_ == "artillery 2")
            {
                return new Artillery2(dc_, pos_, props_, name_);
            }
            if (type_ == "artillery 3")
            {
                return new Artillery3(dc_, pos_, props_, name_);
            }

            if (type_ == "enemy")
            {
                return new EnemyCritter(dc_, pos_, props_, name_);
            }
            if (type_ == "flyenemy")
            {
                return new EnemyFlying(dc_, pos_, props_, name_);
            }
            if (type_ == "fatenemy")
            {
                return new EnemyFat(dc_, pos_, props_, name_);
            }
            if (type_ == "ballenemy")
            {
                return new EnemyBall(dc_, pos_, props_, name_);
            }
            if (type_ == "fastenemy")
            {
                return new EnemyFastboi(dc_, pos_, props_, name_);
            }
            return base.CreateEntity(type_, dc_, pos_, props_, name_);
        }
    }
}
