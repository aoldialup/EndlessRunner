using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class Ground
    {
        private static Ground _instance;

        public static Ground instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Ground();
                }

                return _instance;
            }
        }

        public const int WALL_LEFT_POS_X = 0;

        private Sprite sprite;

        private Ground()
        {
            sprite = new Sprite("assets/ground.png");
            sprite.destRec = new Rectangle(WALL_LEFT_POS_X, 375, sprite.image.Width, sprite.image.Height);
        }

        public static bool IsObjectGrounded(Rectangle r)
        {
            return r.Bottom >= instance.sprite.destRec.Y;
        }

        public static int GetGroundedPositionY(Rectangle r)
        {
            return instance.sprite.destRec.Y - r.Height;
        }

        public void Draw(Graphics gfx)
        {
            sprite.Draw(gfx);
        }
    }
}