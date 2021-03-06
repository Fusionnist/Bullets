﻿using Microsoft.Xna.Framework;
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
        public FontDrawer drawer;
        List<string> texts;
        List<float> poses;
        float scrollSpeed;
        Point virtualDims;
        public bool wasIgnored, isActive;
        public int currentHeight;

        public TextHandler(FontDrawer drawer_, Point virtualDims_)
        {
            drawer = drawer_;
            texts = new List<string>();
            poses = new List<float>();
            scrollSpeed = 30;
            virtualDims = virtualDims_;
            wasIgnored = false;
            currentHeight = 0;
            isActive = true;
        }

        public void Update(float es_)
        {
            if (isActive)
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    poses[i] -= es_ * scrollSpeed;
                    if (poses[i] < -currentHeight)
                    {
                        poses.RemoveAt(i);
                        texts.RemoveAt(i);
                        wasIgnored = true;
                    }
                }
            }
            drawer.Update(es_);
        }

        public void Draw(SpriteBatch sb_)
        {
            if (isActive)
            {
                for (int i = 0; i < texts.Count; i++)
                    currentHeight = drawer.DrawText("aaa", texts[i], new Rectangle(10, (int)Math.Round(poses[i]), virtualDims.X * 2 / 5 - 12, 666), sb_).Height;
            }
        }

        public void AddTextToScroll(string textToAdd_, int scrollSpd_)
        {
            texts.Add(textToAdd_);
            poses.Add(virtualDims.Y);
            scrollSpeed = scrollSpd_;
            isActive = true;
        }

        public void RemoveText()
        {
            if (texts.Count != 0)
            {
                poses.RemoveAt(0);
                texts.RemoveAt(0);
            }
        }
    }
}
