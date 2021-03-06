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
    public class Event
    {
        public string text;
        public string[] outcomeIfYes, outcomeIfNo, outcomeIfIgnored;
        bool wasIgnored;
        public int scrollSpeed;
        public TextureDrawer tex;

        public Event(string text_, string[] outcomeIfYes_, string[] outcomeIfNo_, string[] outcomeIfIgnored_, int scrollSpeed_, TextureDrawer tex_ = null)
        {
            text = text_;
            outcomeIfYes = outcomeIfYes_;
            outcomeIfNo = outcomeIfNo_;
            outcomeIfIgnored = outcomeIfIgnored_;
            scrollSpeed = scrollSpeed_;
            tex = tex_;
        }
    }
}
