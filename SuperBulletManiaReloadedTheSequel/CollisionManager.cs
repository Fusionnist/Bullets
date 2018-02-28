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

        public void DoBounds(Player player_)
        {
            PlayerAndWall(player_, true, 0);
            PlayerAndWall(player_, true, 1440);
            PlayerAndWall(player_, false, 0);
            PlayerAndWall(player_, false, 810);
        }
    }
}
