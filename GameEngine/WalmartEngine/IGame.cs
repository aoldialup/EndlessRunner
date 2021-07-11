using System.Drawing;

namespace WalmartEngine
{
    interface IGame
    {
        void LoadContent(GameContainer gc);
        void Update(GameContainer gc, float deltaTime);
        void Draw(GameContainer gc, Graphics gfx);
    }
}
