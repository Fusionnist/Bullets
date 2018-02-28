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
                if (hb.X + hb.Width < pos_ && pos_ < hb.MovX().X + hb.Width)
                    player_.mov.X -= hb.MovX().X + hb.Width - pos_;
                else if (hb.X > pos_ && pos_ > hb.MovX().X)
                    player_.mov.X += pos_ - hb.MovX().X;
            }
        }
    }
}
