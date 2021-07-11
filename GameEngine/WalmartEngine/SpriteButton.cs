using System.Drawing;
using WalmartEngine;

namespace PlainEngine
{
    class SpriteButton
    {
        private Sprite button;

        public bool isVisible { get; set; }
        public bool isActive { get; set; }

        public SpriteButton(string filepath, Point position, bool isVisible, bool isActive)
        {
            button = new Sprite(filepath);
            button.destRec = new Rectangle(position, new Size(button.image.Width, button.image.Height));

            this.isVisible = isVisible;
            this.isActive = isActive;
        }

        public void Draw(Graphics gfx)
        {
            if (isVisible)
            {
                button.Draw(gfx);
            }
        }
    }
}