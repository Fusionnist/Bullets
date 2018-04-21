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
    class TextHandler
    {
        FontDrawer drawer;
        List<string> texts;
        List<float> poses;
        float scrollSpeed;
        Point virtualDims;

        public TextHandler(FontDrawer drawer_, Point virtualDims_)
        {
            drawer = drawer_;
            texts = new List<string>();
            poses = new List<float>();
            scrollSpeed = -50;
            virtualDims = virtualDims_;
        }

        public void Update(float es_)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                poses[i] += es_ * scrollSpeed;
                if (poses[i] >= virtualDims.Y * 7/8)
                {
                    poses.RemoveAt(i);
                    texts.RemoveAt(i);
                }
            }
            drawer.Update(es_);
        }

        public void Draw(SpriteBatch sb_)
        {
            for (int i = 0; i < texts.Count; i++)
                drawer.DrawText("aaa", texts[i], new Rectangle(virtualDims.X * 3 / 5, (int)poses[i], virtualDims.X * 2 / 5, 666), sb_);
        }

        public void AddTextToScroll(string textToAdd_)
        {
            texts.Add(textToAdd_);
            poses.Add(virtualDims.Y * 4 / 5);
        }
    }
}
