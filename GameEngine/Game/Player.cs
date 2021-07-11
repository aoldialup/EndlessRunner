using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WalmartEngine;

namespace WalmartMario
{
    class Player
    {
        public const int STAND_STATE = 0;
        public const int DUCK_STATE = 1;
        public const int JUMP_STATE = 2;

        public const int STATES_COUNT = 3;

        private const float MAX_DUCK_TIME_SECONDS = 1.5f;

        private const float SCORE_MULTIPLIER_INCREMENT = 0.5f;
        private const int STREAK_REQUIRED_FOR_SCORE_MULTIPLIER = 5;

        private const float NO_SCORE_MULTIPLIER = 1f;

        private static Player _instance;

        private Sprite[] states;
        private int currentStateIndex;

        public Sprite currentState
        {
            get
            {
                return states[currentStateIndex];
            }
        }

        private Vector2 speed = Vector2.zero;

        private float jumpSpeed = 5f;

        private float timeDucking = 0f;

        private PlayerStats playerStats;

        private PlayerInsulter playerInsulter;

        private int currentRound;

        private string statsFilepath = "stats.txt";

        public int playerStandDestRecRight { get; private set; }

        private Sprite[] heartContainers;

        private UIText highScoreText;
        private UIText scoreText;
        private UIText currentRoundText;
        private UIText killStreakText;

        public static Player instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Player();
                }

                return _instance;
            }
        }

        private Player()
        {
            InitStates();
            InitHeartContainers();
            InitStats();

            LoadHighScore();
            InitUIElements();
            InitPlayer();

            playerInsulter = new PlayerInsulter();
        }

        private void InitStates()
        {
            string[] filepaths = { "standing.png", "ducking.png", "jump.png" };

            states = new Sprite[STATES_COUNT];

            for (int i = 0; i <= STATES_COUNT - 1; i++)
            {
                states[i] = new Sprite($"assets/player_states/{filepaths[i]}");
                states[i].destRec = new Rectangle(0, 0, states[i].image.Width, states[i].image.Height);
                states[i].truePosition = new Vector2(0f, Ground.GetGroundedPositionY(states[i].destRec));
            }
        }

        private void InitPlayer()
        {
            playerStandDestRecRight = states[STAND_STATE].destRec.Right;

            currentStateIndex = STAND_STATE;
        }

        private void InitHeartContainers()
        {
            heartContainers = new Sprite[PlayerStats.MAX_HEALTH];

            for (int i = 0; i <= PlayerStats.MAX_HEALTH - 1; i++)
            {
                heartContainers[i] = new Sprite("assets/heart_container.png");
                heartContainers[i].destRec = new Rectangle(50 * i, 0,
                    heartContainers[i].image.Width / 2, heartContainers[i].image.Height / 2);
            }
        }

        private void LoadHighScore()
        {
            try
            {
                string highScoreString = File.ReadAllText(statsFilepath);

                bool success = int.TryParse(highScoreString, out playerStats.highScore);

                if (success)
                {
                    Log.Info(this, "Loaded stats");
                }
                else
                {
                    Log.Info(this, "Couldn't load stats");
                }
            }
            catch (IOException)
            {
                File.Create("stats.txt");
                playerStats.highScore = 0;

                Log.Info(this, "Failed to load stats; created file");
            }
        }

        private int CalcRoundCompletion()
        {
            double completion = ((double)playerStats.killCount / playerStats.killsNeededCount) * 100;

            return (int)completion;
        }

        private void InitStats()
        {
            currentRound = 1;

            playerStats = new PlayerStats
            {
                currentScore = 0,
                killStreak = 0,
                scoreMultiplier = NO_SCORE_MULTIPLIER,
                killsNeededCount = Obstacle.MIN_KILLS_NEEDED * currentRound,
                killCount = 0,

                health = PlayerStats.MAX_HEALTH,
                isDead = false
            };
        }

        private void InitUIElements()
        {
            scoreText = new UIText($"SCORE: {playerStats.currentScore} (x{playerStats.scoreMultiplier})", new Font("Calibri", 16), new Point(0, 50), Color.White);
            highScoreText = new UIText($"HIGH SCORE: {playerStats.highScore}", new Font("Calibri", 16), new Point(0, 80), Color.White);
            currentRoundText = new UIText($"ROUND {currentRound}: 0%", new Font("Calibri", 16), new Point(200, 80), Color.White);
            killStreakText = new UIText("KILLSTREAK: 0", new Font("Calibri", 16), new Point(200, 50), Color.White);

        }

        private void ChangeState(int multiplier, int newState)
        {
            float yOffset = multiplier * Math.Abs(states[currentStateIndex].truePosition.y - states[newState].truePosition.y);

            states[newState].truePosition = new Vector2(0f, Ground.GetGroundedPositionY(states[currentStateIndex].destRec) + yOffset);

            currentStateIndex = newState;
        }

        public void Update(GameContainer gc, float deltaTime)
        {
            speed += new Vector2(0f, Physics.GRAVITY);

            if (Input.IsKeyClicked(Keys.C) && currentStateIndex == STAND_STATE)
            {
                Duck();
            }

            if (Input.IsKeyClicked(Keys.Space) && currentStateIndex != JUMP_STATE)
            {
                Jump();
            }

            if (currentStateIndex == JUMP_STATE)
            {
                states[currentStateIndex].truePosition += speed;
            }
            else if (currentStateIndex == DUCK_STATE)
            {
                if (timeDucking >= MAX_DUCK_TIME_SECONDS)
                {
                    ChangeState(-1, STAND_STATE);
                    timeDucking = 0f;
                }
                else
                {
                    timeDucking += Helper.ToSeconds(deltaTime);
                }
            }

            if (ShouldBeInStandState())
            {
                ChangeState(-1, STAND_STATE);
                speed = Vector2.zero;

                SetPositionToGround();
            }

            if (Input.IsKeyClicked(Keys.X))
            {
                Projectile.instance.Fire(new Vector2(currentState.destRec.Right, currentState.destRec.Top));
            }

            Projectile.instance.Update(deltaTime);
        }

        private bool ShouldBeInStandState()
        {
            return IsGrounded() && currentStateIndex == JUMP_STATE;
        }

        private void SetPositionToGround()
        {
            states[currentStateIndex].truePosition = new Vector2(0f, Ground.GetGroundedPositionY(states[currentStateIndex].destRec));
        }

        public void SaveHighScore()
        {
            File.WriteAllText(statsFilepath, playerStats.currentScore.ToString());
            Log.Info(this, "Updated high score");
        }

        private void UpdateKillStreakText()
        {
            killStreakText.text = $"KILLSTREAK: {playerStats.killStreak}";
        }

        private void UpdateScoreText()
        {
            scoreText.text = $"SCORE: {playerStats.currentScore} (x{playerStats.scoreMultiplier})";
        }

        private bool IsGrounded()
        {
            return Ground.IsObjectGrounded(states[currentStateIndex].destRec);
        }

        public void TakeDamage()
        {
            heartContainers[playerStats.health - 1].isVisible = false;
            playerStats.health--;

            if (playerStats.health > 0)
            {
                ResetKillStreak();
            }
            else
            {
                DeadLogic();
            }
        }

        public void ResetKillStreak()
        {
            playerStats.killStreak = 0;
            playerStats.scoreMultiplier = NO_SCORE_MULTIPLIER;

            UpdateKillStreakText();
            UpdateScoreText();
        }

        private void DeadLogic()
        {
            playerStats.isDead = true;

            highScoreText.font = new Font("Calibri", 17);
            highScoreText.position = new Point(0, 0);

            playerStats.highScoreBeaten = playerStats.currentScore > playerStats.highScore;

            SetupInsult();
        }

        private void SetupInsult()
        {
            string insult = playerInsulter.GetInsult(playerStats);

            if (playerStats.highScoreBeaten)
            {
                SaveHighScore();

                highScoreText.text = $"{insult}\n\nNEW HIGH SCORE: {playerStats.currentScore}\nOLD HIGH SCORE: {playerStats.highScore}\n\nESC to exit";
            }
            else
            {
                highScoreText.text = $"{insult}\n\nSCORE: {playerStats.currentScore}\nHIGH SCORE: {playerStats.highScore}\n\nESC to exit";
            }
        }

        private void AddToCurrentScore()
        {
            playerStats.currentScore += (int)(Obstacle.KILL_SCORE * playerStats.scoreMultiplier);
        }

        private bool RoundOver()
        {
            return playerStats.killCount == playerStats.killsNeededCount;
        }

        private void UpdateRoundData()
        {
            playerStats.killCount++;

            if (RoundOver())
            {
                Projectile.instance.RoundEndLogic();
                PlayerCollidableEntityManager.instance.RoundEndLogic();
                RoundEndLogic();
            }

            currentRoundText.text = $"ROUND: {currentRound}: {CalcRoundCompletion()}%";
            UpdateScoreText();
        }

        private void RoundEndLogic()
        {
            timeDucking = 0f;

            currentRound++;

            playerStats.killsNeededCount = Obstacle.MIN_KILLS_NEEDED * currentRound;
            playerStats.killCount = 0;

            playerStats.scoreMultiplier = NO_SCORE_MULTIPLIER;
            playerStats.killStreak = 0;
        }

        private void KillStreakLogic()
        {
            playerStats.killStreak++;

            if (playerStats.killStreak == STREAK_REQUIRED_FOR_SCORE_MULTIPLIER)
            {
                playerStats.scoreMultiplier += SCORE_MULTIPLIER_INCREMENT;
                playerStats.killStreak = 0;
            }

            UpdateKillStreakText();
        }

        public void AddScore()
        {
            AddToCurrentScore();
            UpdateRoundData();
            KillStreakLogic();
        }

        private void Jump()
        {
            ChangeState(-1, JUMP_STATE);
            speed = Vector2.one * -jumpSpeed;
        }

        private void Duck()
        {
            ChangeState(1, DUCK_STATE);
            timeDucking = 0f;
        }

        public void Draw(Graphics gfx)
        {
            scoreText.Draw(gfx);
            highScoreText.Draw(gfx);
            killStreakText.Draw(gfx);
            currentRoundText.Draw(gfx);

            foreach (Sprite hc in heartContainers)
            {
                if (hc.isVisible)
                {
                    hc.Draw(gfx);
                }
            }

            currentState.Draw(gfx);

            Projectile.instance.Draw(gfx);
        }

        public void DisplayFinalScore(Graphics gfx)
        {
            highScoreText.Draw(gfx);
        }

        public bool IsDead()
        {
            return playerStats.isDead;
        }

        public void AddHealth()
        {
            if (playerStats.health < PlayerStats.MAX_HEALTH)
            {
                heartContainers[playerStats.health].isVisible = true;
                playerStats.health++;
            }
        }

        public void Kill()
        {
            DeadLogic();
        }
    }
}