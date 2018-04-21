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
    public class Map
    {
        public Vector2[] points;
        TextureDrawer ground;
        public FRectangle[] buildableAreas;

        public Map(Vector2[] points_, TextureDrawer ground_, FRectangle[] buildableAreas_)
        {
            points = points_;
            ground = ground_;
            buildableAreas = buildableAreas_;
        }

        public Vector2 NextPoint(Vector2 entPos_)
        {
            int closestIndex = 0;
            float record = 10000;
            for(int x = 0; x < points.Length; x++)
            {
                if((points[x]-entPos_).Length() < record)
                {
                    closestIndex = x;
                    record = (points[x] - entPos_).Length();
                }
            }
            if(closestIndex + 1 != points.Length)
                return points[closestIndex + 1];
            else { return points[closestIndex]; }
        }

        public void Update(float es_)
        {
            //update ground texture
            ground.Update(es_);
            //direct enemies
            foreach (Entity e in EntityCollection.GetGroup("enemy"))
            {
                e.FeedVector(NextPoint(e.pos), "path");
            }
        }

        public void Draw(SpriteBatch sb_)
        {
            //draw map
            ground.Draw(sb_, Vector2.Zero);
        }
    }
}
