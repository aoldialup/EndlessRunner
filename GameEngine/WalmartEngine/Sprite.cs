using System.Drawing;

namespace WalmartEngine
{
    class Sprite
    {
        private Rectangle _destRec;

        private Vector2 _truePosition = Vector2.zero;

        public bool isVisible { get; set; } = true;

        public Bitmap image { get; private set; }

        public Rectangle destRec
        {
            get
            {
                return _destRec;
            }
            set
            {
                _destRec = value;

                _truePosition = new Vector2(_destRec.X, _destRec.Y);
            }
        }

        public Vector2 truePosition
        {
            get
            {
                return _truePosition;
            }
            set
            {
                _truePosition = value;

                _destRec.X = (int)_truePosition.x;
                _destRec.Y = (int)_truePosition.y;
            }
        }



        public Sprite(string filepath)
        {
            image = Helper.LoadImage(filepath);
            truePosition = Vector2.zero;
        }

        public void Draw(Graphics gfx)
        {
            if (isVisible)
            {
                gfx.DrawImageUnscaledAndClipped(image, destRec);
            }
        }
    }
}
