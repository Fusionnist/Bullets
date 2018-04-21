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

        public Enemy(DrawerCollection t_, Vector2 p_, List<Property> prop_) : base(t_,p_,prop_,"enemi","enemy")
        {
            point = pos;
            deathTimer = new Timer(10);
            hp = 100;
            r = new Random();
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
            mov += v;
        }

        void Wiggle()
        {
            //WIGGLE
            foreach (Entity e in EntityCollection.GetGroup("enemies"))
            {
                if ((e.pos - pos).Length() < 4)
                {
                    float a = (float)r.NextDouble() * (float)Math.PI*2;
                    Vector2 apart = new Vector2((float)Math.Sin(a), (float)Math.Cos(a));
                    e.mov += apart;
                    mov -= apart;
                }
            }
        }

        public override void Update(float elapsedTime_)
        {
            if(hp < 0)
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
