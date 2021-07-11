using System.Drawing;

namespace WalmartEngine
{
    class UIText
    {
        public Font font { get; set; }

        public Point position { get; set; }

        public string text { get; set; }

        public Color color { get; set; }

        public UIText(string text, Font font, Point position, Color color)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;
        }

        public void Draw(Graphics gfx)
        {
            gfx.DrawString(text, font, new SolidBrush(color), position.X, position.Y);
        }
    }
}