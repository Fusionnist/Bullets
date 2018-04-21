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
    class Event
    {
        string text, outcomeIfYes, outcomeIfNo, outcomeIfIgnored;
        bool wasIgnored;

        public Event(string text_, string outcomeIfYes_, string outcomeIfNo_, string outcomeIfIgnored_)
        {
            text = text_;
            outcomeIfYes = outcomeIfYes_;
            outcomeIfNo = outcomeIfNo_;
            outcomeIfIgnored = outcomeIfIgnored_;
        }
    }
}
