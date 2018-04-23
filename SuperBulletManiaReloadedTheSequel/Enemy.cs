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
    public class Enemy : Entity
    {
        //next pt in map
        Vector2 point;
        Timer deathTimer;
        Random r;
        bool looted;
        protected float speed;
        protected int worth;

        public Enemy(DrawerCollection t_, Vector2 p_, List<Property> prop_, string name_) : base(t_,p_,prop_,name_,"enemy")
        {
            point = pos;
            deathTimer = new Timer(1);
            hp = 100;
            worth = 1;
            r = new Random();
        }

        public override int GetValue(string context_)
        {
            if (context_ == "loot" && isDestroyed)
            {
                if (looted) { return 0; }
                else { looted = true; return 1; }
            }
            return base.GetValue(context_);
        }

        public override void FeedVector(Vector2 vec_, string context_)
        {
            //REGISTER PATH POINT
            if(context_ == "path")
            {
                if((point-pos).Length() < 10)
                point = vec_;
            }
            base.FeedVector(vec_, context_);
        }

        public override void Move()
        {
            if (!isDestroyed)
            {
                FollowPoint();
                Wiggle();
            }
            base.Move();
        }

        void FollowPoint()
        {
            //FOLLOW PATH
            Vector2 v = (point - pos);
            v.Normalize();
            mov += v*speed;
        }

        void Wiggle()
        {
            //WIGGLE
            foreach (Entity e in EntityCollection.GetGroup("enemies"))
            {
                if ((e.pos - pos).Length() < 5)
                {
                    Vector2 diff = e.pos - pos;
                    float a = (float)Math.Atan2(diff.Y, diff.X);
                    Vector2 apart = new Vector2((float)Math.Cos(a), (float)Math.Sin(a))/5;
                    e.mov += apart;
                    mov -= apart;
                }
            }
        }

        public override void Update(float elapsedTime_)
        {
            if(hp <= 0)
            {
                isDestroyed = true;
                deathTimer.Update(elapsedTime_);
                if (deathTimer.Complete())
                {
                    exists = false;
                }
            }
            base.Update(elapsedTime_);
        }
    }
}
