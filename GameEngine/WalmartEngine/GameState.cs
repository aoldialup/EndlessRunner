using System;
using System.Drawing;

namespace WalmartEngine
{
    class GameState
    {
        public Action<GameContainer, float> Update;

        public Action<Graphics> Draw;

        public GameState(Action<GameContainer, float> update, Action<Graphics> draw)
        {
            this.Draw = draw;
            this.Update = update;
        }
    }
}