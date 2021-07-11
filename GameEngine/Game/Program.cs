using WalmartMario;

namespace WalmartEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            GameContainer gc = new GameContainer(512, 600, "Walmart Mario", new Game());
        }
    }
}
