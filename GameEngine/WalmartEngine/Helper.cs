
using System;
using System.Drawing;
namespace WalmartEngine
{
    class Helper
    {
        private static Random random = new Random();

        public static int GetRandom(int min, int max)
        {
            return random.Next(min, max);
        }

        public static Bitmap LoadImage(string filepath)
        {
            Image temp = Image.FromFile(filepath);
            Bitmap sprite = new Bitmap(temp, temp.Width, temp.Height);

            return sprite;
        }

        public static float ToSeconds(float ms)
        {
            return ms / 1000f;
        }
    }
}
