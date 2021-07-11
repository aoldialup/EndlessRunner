using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WalmartEngine
{
    class Input
    {
        private static int keysCount;

        private static Dictionary<Keys, bool> keysCurrent;
        private static Dictionary<Keys, bool> keysLast;

        public Input()
        {
            keysCount = Enum.GetNames(typeof(Keys)).Length;

            keysCurrent = new Dictionary<Keys, bool>(keysCount);
            keysLast = new Dictionary<Keys, bool>(keysCount);

            for (int i = 0; i < keysCount; i++)
            {
                keysCurrent.Add((Keys)i, false);
            }
        }

        public static bool IsKeyDown(Keys key)
        {
            return keysCurrent[key];
        }

        public static bool IsKeyClicked(Keys key)
        {
            return keysCurrent[key] && !keysLast[key];
        }

        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            keysCurrent[e.KeyCode] = true;
        }

        public void Window_KeyUp(object sender, KeyEventArgs e)
        {
            keysCurrent[e.KeyCode] = false;
        }

        public void Update()
        {
            for (int i = 0; i < keysCount; i++)
            {
                keysLast[(Keys)i] = keysCurrent[(Keys)i];
            }
        }
    }
}