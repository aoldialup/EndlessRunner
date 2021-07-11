using System.Drawing;

namespace WalmartMario
{
    interface IMovable
    {
        void Spawn();

        bool Update(float deltaTime);


        void Draw(Graphics gfx);
    }
}
