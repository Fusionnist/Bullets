using MonoGame.FZT;

namespace SuperBulletManiaReloadedTheSequel
{
    class CollisionManager
    {
        public CollisionManager() { }

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
            PlayerAndWall(player_, true, 1440);
            PlayerAndWall(player_, false, 0);
            PlayerAndWall(player_, false, 810);

            foreach (var bullet in bullets_)
            {
                BulletAndWall(bullet, true, 0);
                BulletAndWall(bullet, true, 1440);
                BulletAndWall(bullet, false, 0);
                BulletAndWall(bullet, false, 810);
            }
        }
    }
}
