using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WalmartEngine
{
    class GameContainer
    {
        public int screenWidth { get; }
        public int screenHeight { get; }
        public string screenTitle { get; }

        private IGame game;

        private Thread gameThread;

        public Input input;

        private Canvas window;

        private bool isRunning = false;

        private const int CURRENT_FPS = 60;

        private int actualFPS = 0;

        public GameContainer(int screenWidth, int screenHeight, string screenTitle, IGame game)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.screenTitle = screenTitle;
            this.game = game;

            input = new Input();

            InitWindow();

            gameThread = new Thread(new ThreadStart(Run));
            gameThread.Start();

            Application.Run(window);
        }

        private void Run()
        {
            isRunning = true;

            const float GAME_FREQUENCY = CURRENT_FPS;

            const float TIME_BETWEEN_UPDATES = 1000000000 / GAME_FREQUENCY;
            const int MAX_UPDATES_BEFORE_RENDER = 2;

            float lastUpdateTime = NanoTime();
            float lastRenderTime = lastUpdateTime;

            const float TARGET_TIME_BETWEEN_RENDERS = 1000000000 / CURRENT_FPS;

            int lastSecondTime = (int)(lastUpdateTime / 1000000000);

            float now = 0;
            int updateCount = 0;
            float delta = 0f;
            int frameCount = 0;
            bool render = false;

            int thisSecond = 0;

            LoadContent();

            while (isRunning)
            {
                now = NanoTime();
                updateCount = 0;
                render = false;

                while (now - lastUpdateTime > TIME_BETWEEN_UPDATES && updateCount < MAX_UPDATES_BEFORE_RENDER)
                {
                    delta = (now - lastUpdateTime) / 1000000;

                    Update(delta);
                    input.Update();

                    lastUpdateTime += TIME_BETWEEN_UPDATES;
                    updateCount++;
                    render = true;
                }

                if (render)
                {
                    ClearAndDraw();
                    frameCount++;
                    lastRenderTime = now;
                }

                thisSecond = (int)(lastUpdateTime / 1000000000);
                if (thisSecond > lastSecondTime)
                {
                    actualFPS = frameCount;
                    frameCount = 0;
                    lastSecondTime = thisSecond;
                }

                while (now - lastRenderTime < TARGET_TIME_BETWEEN_RENDERS &&
       now - lastUpdateTime < TIME_BETWEEN_UPDATES)
                {
                    Thread.Yield();

                    now = NanoTime();
                }
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                Application.Exit();
            }
        }

        private void Window_OnCloseEvent(object sender, EventArgs e)
        {
            Stop();
        }

        private void ClearAndDraw()
        {
            window.BeginInvoke((MethodInvoker)delegate
            {
                window.Refresh();
            });
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;

            gfx.Clear(Color.CornflowerBlue);

            game.Draw(this, gfx);
        }

        private void LoadContent()
        {
            game.LoadContent(this);
        }

        private void Update(float deltaTime)
        {
            game.Update(this, deltaTime);
        }

        private void InitWindow()
        {
            window = new Canvas();
            window.Size = new Size(screenWidth, screenHeight);
            window.Text = screenTitle;

            window.Paint += Draw;
            window.KeyDown += input.Window_KeyDown;
            window.KeyUp += input.Window_KeyUp;
            window.OnCloseEvent += Window_OnCloseEvent;
        }

        private long NanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
    }
}