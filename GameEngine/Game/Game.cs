using PlainEngine;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WalmartEngine;

namespace WalmartMario
{
    class Game : IGame
    {
        private StateManager stateManager;

        private const int MENU_STATE = 0;
        private const int GAMEPLAY_STATE = 1;
        private const int INSTRUCTIONS_STATE = 2;
        private const int DEAD_STATE = 3;
        private const int ACTUAL_INSTRUCTIONS_STATE = 4;

        private PlayerCollidableEntityManager entityManager;
        private Player player;
        private Ground ground;

        private SpriteButton playButton;
        private SpriteButton instructionsButton;
        private SpriteButton actualInstructionsButton;
        private SpriteButton exitButton;

        private UIText jokeInstructionsText;
        private UIText actualInstructionsText;
        private UIText titleText;

        private bool hasSeenJokeInstructions;

        private void InitMenu()
        {
            string jokeInstructions = File.ReadAllText("assets/instructions.txt");
            string actualInstructions = File.ReadAllText("assets/actual_instructions.txt");

            playButton = new SpriteButton("assets/buttons/play.png", new Point(200, 100), true, true);
            instructionsButton = new SpriteButton("assets/buttons/instructions.png", new Point(200, 200), true, true);
            exitButton = new SpriteButton("assets/buttons/exit.png", new Point(200, 300), true, true);
            actualInstructionsButton = new SpriteButton("assets/buttons/actual_instructions.png", new Point(200, 400), true, true);

            jokeInstructionsText = new UIText(jokeInstructions, new Font("Calibri", 7), new Point(0, 0), Color.White);
            actualInstructionsText = new UIText(actualInstructions, new Font("Calibri", 8), new Point(0, 0), Color.White);
            titleText = new UIText("Walmart Mario", new Font("Calibri", 16), new Point(200, 50), Color.Red);
        }

        public void LoadContent(GameContainer gc)
        {
            InitStates();
            InitMenu();

            entityManager = PlayerCollidableEntityManager.instance;
            player = Player.instance;
            ground = Ground.instance;

            stateManager.ChangeState(MENU_STATE);
        }

        public void Update(GameContainer gc, float deltaTime)
        {
            stateManager.UpdateCurrentState(gc, deltaTime);
        }

        public void Draw(GameContainer gc, Graphics gfx)
        {
            stateManager.DrawCurrentState(gfx);
        }

        private void InitStates()
        {
            stateManager = new StateManager();

            stateManager.AddState(MENU_STATE, UpdateMenuState, DrawMenuState);
            stateManager.AddState(INSTRUCTIONS_STATE, UpdateInstructionsState, DrawInstructionsState);
            stateManager.AddState(GAMEPLAY_STATE, UpdateGameplayState, DrawGameplayState);
            stateManager.AddState(DEAD_STATE, UpdateDeadState, DrawDeadState);
            stateManager.AddState(ACTUAL_INSTRUCTIONS_STATE, UpdateActualInstructionsState, DrawActualInstructionsState);
        }

        private void UpdateGameplayState(GameContainer gc, float deltaTime)
        {
            player.Update(gc, deltaTime);
            entityManager.Update(deltaTime);

            if (player.IsDead())
            {
                stateManager.ChangeState(DEAD_STATE);
            }
        }

        private void DrawGameplayState(Graphics gfx)
        {
            ground.Draw(gfx);
            player.Draw(gfx);
            entityManager.Draw(gfx);
        }

        private void UpdateMenuState(GameContainer gc, float deltaTime)
        {
            if (Input.IsKeyDown(Keys.D1))
            {
                stateManager.ChangeState(GAMEPLAY_STATE);
            }
            else if (Input.IsKeyDown(Keys.D2))
            {
                stateManager.ChangeState(INSTRUCTIONS_STATE);
                hasSeenJokeInstructions = true;
            }
            else if (Input.IsKeyDown(Keys.D3))
            {
                gc.Stop();
            }
            else if (Input.IsKeyDown(Keys.D4) && hasSeenJokeInstructions)
            {
                stateManager.ChangeState(ACTUAL_INSTRUCTIONS_STATE);
            }
        }

        private void DrawMenuState(Graphics gfx)
        {
            playButton.Draw(gfx);
            instructionsButton.Draw(gfx);
            exitButton.Draw(gfx);
            actualInstructionsButton.Draw(gfx);

            titleText.Draw(gfx);
        }

        private void UpdateInstructionsState(GameContainer gc, float deltaTime)
        {
            if (Input.IsKeyClicked(Keys.Escape))
            {
                stateManager.ChangeState(MENU_STATE);
            }
        }

        private void DrawInstructionsState(Graphics gfx)
        {
            jokeInstructionsText.Draw(gfx);
        }

        private void UpdateActualInstructionsState(GameContainer gc, float deltaTime)
        {
            if (Input.IsKeyClicked(Keys.Escape))
            {
                stateManager.ChangeState(MENU_STATE);
            }
        }

        private void DrawActualInstructionsState(Graphics gfx)
        {
            actualInstructionsText.Draw(gfx);
        }

        private void UpdateDeadState(GameContainer gc, float deltaTime)
        {
            if (Input.IsKeyClicked(Keys.Escape))
            {
                gc.Stop();
            }
        }

        private void DrawDeadState(Graphics gfx)
        {
            player.DisplayFinalScore(gfx);
        }
    }
}