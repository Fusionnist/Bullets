using MonoGame.FZT;

namespace SuperBulletManiaReloadedTheSequel
{
    class CollisionManager
    {
        int boundWidth, boundHeight;

        public CollisionManager(int boundWidth_, int boundHeight_)
        {
            boundWidth = boundWidth_;
            boundHeight = boundHeight_;
        }

        public void PlayerAndWall(Player player_, bool isVertical_, float pos_)
        {
            FRectangle hb = player_.MovHB()[0];
            if (isVertical_)
            {
                if (hb.X + hb.Width <= pos_ && pos_ < hb.X + player_.mov.X + hb.Width)
                    player_.mov.X -= hb.X + player_.mov.X + hb.Width - pos_;
                else if (hb.X >= pos_ && pos_ > hb.X + player_.mov.X)
                    player_.mov.X += pos_ - hb.X - player_.mov.X;
            }
            else
            {
                if (hb.Y + hb.Height <= pos_ && pos_ < hb.Y + player_.mov.Y + hb.Height)
                    player_.mov.Y -= hb.Y + player_.mov.Y + hb.Height - pos_;
                else if (hb.Y >= pos_ && pos_ > hb.Y + player_.mov.Y)
                    player_.mov.Y += pos_ - hb.Y - player_.mov.Y;
            }
        }

        public void BulletAndWall(Bullet bullet_, bool isVertical_, float pos_)
        {
            FRectangle hb = bullet_.MovHB()[0];
            if (isVertical_)
            {
                if (hb.X + hb.Width <= pos_ && pos_ < hb.X + bullet_.mov.X + hb.Width)
                { bullet_.mov.X -= hb.X + bullet_.mov.X + hb.Width - pos_; bullet_.Bounce(true); }
                else if (hb.X >= pos_ && pos_ > hb.X + bullet_.mov.X)
                { bullet_.mov.X += pos_ - hb.X - bullet_.mov.X; bullet_.Bounce(true); }
            }
            else
            {
                if (hb.Y + hb.Height <= pos_ && pos_ < hb.Y + bullet_.mov.Y + hb.Height)
                { bullet_.mov.Y -= hb.Y + bullet_.mov.Y + hb.Height - pos_; bullet_.Bounce(false); }
                else if (hb.Y >= pos_ && pos_ > hb.Y + bullet_.mov.Y)
                { bullet_.mov.Y += pos_ - hb.Y - bullet_.mov.Y; bullet_.Bounce(false); }
            }
        }

        public void DoBounds(Player player_, Bullet[] bullets_)
        {
            PlayerAndWall(player_, true, 0);
            PlayerAndWall(player_, true, boundWidth);
            PlayerAndWall(player_, false, 0);
            PlayerAndWall(player_, false, boundHeight);

            foreach (var bullet in bullets_)
            {
                BulletAndWall(bullet, true, 0);
                BulletAndWall(bullet, true, boundWidth);
                BulletAndWall(bullet, false, 0);
                BulletAndWall(bullet, false, boundHeight);
            }
        }

        public void TileAndPlayer(Tile tile_, Player player_)
        {
            FRectangle hb = player_.MovHB()[0];
            FRectangle tilehb = tile_.MovHB()[0];
            if (hb.X + player_.mov.X < tilehb.X + tilehb.Width && hb.X + player_.mov.X + hb.Width > tilehb.X)
            { PlayerAndWall(player_, false, tilehb.Y); PlayerAndWall(player_, false, tilehb.Y + tilehb.Height); }
            if (hb.Y + player_.mov.Y < tilehb.Y + tilehb.Height && hb.Y + player_.mov.Y + hb.Height > tilehb.Y)
            { PlayerAndWall(player_, true, tilehb.X); PlayerAndWall(player_, true, tilehb.X + tilehb.Width); }
        }

        public void TileAndBullet(Bullet bullet_, Tile tile_)
        {
            FRectangle bullethb = bullet_.MovHB()[0];
            FRectangle tilehb = tile_.MovHB()[0];
            if (bullethb.X + bullet_.mov.X < tilehb.X + tilehb.Width && bullethb.X + bullet_.mov.X + bullethb.Width > tilehb.X)
            { BulletAndWall(bullet_, false, tilehb.Y); BulletAndWall(bullet_, false, tilehb.Y + tilehb.Height); }
            if (bullethb.Y + bullet_.mov.Y < tilehb.Y + tilehb.Height && bullethb.Y + bullet_.mov.Y + bullethb.Height > tilehb.Y)
            { BulletAndWall(bullet_, true, tilehb.X); BulletAndWall(bullet_, true, tilehb.X + tilehb.Width); }
        }
    }
}
