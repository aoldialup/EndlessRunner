using System;
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class PlayerCollidableEntityManager
    {
        public const bool CONTINUE_UPDATING_ENTITY = true;

        private const float SLOWEST_SPAWN_RATE = 1f;
        private const float HIGHEST_SPAWN_RATE = 0.5f;

        private const float SPAWN_RATE_INCREASE = 0.05f;
        private const int SPAWN_CHANCE_INCREASE = 5;

        private const int MAX_SPAWN_CHANCE = 90;
        private const int MIN_SPAWN_CHANCE = 50;

        public const int START_POS_X = 512;

        private int currentObstacleSpawnChance = MIN_SPAWN_CHANCE;
        private float currentEntitySpawnRate = SLOWEST_SPAWN_RATE;

        private static PlayerCollidableEntityManager _instance;

        public bool isEntityAlive { get; private set; }

        public bool isCurrentEntityPowerup { get; private set; }

        private float timeUntilEntitySpawn;

        private Powerup powerup;
        private Obstacle obstacle;

        private UIText timeUntilEntitySpawnText;

        public IMovable currentEntity
        {
            get
            {
                if (isCurrentEntityPowerup)
                {
                    return powerup;
                }

                return obstacle;
            }
        }

        public static PlayerCollidableEntityManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerCollidableEntityManager();
                }

                return _instance;
            }
        }

        private PlayerCollidableEntityManager()
        {
            InitUI();
        }

        public void Update(float deltaTime)
        {
            GenerateEntity(deltaTime);

            EntityPhysics(deltaTime);

            PowerupUIManager.instance.Update(deltaTime);
        }

        private void InitUI()
        {
            timeUntilEntitySpawnText = new UIText(string.Empty, new Font("Calibri", 16), new Point(160, 15), Color.White);

            timeUntilEntitySpawn = currentEntitySpawnRate;
        }

        private bool SpawnChanceAdequate()
        {
            int chance = Helper.GetRandom(0, 100);

            return chance < currentObstacleSpawnChance + 1;
        }

        private void SpawnEntity()
        {
            isCurrentEntityPowerup = Convert.ToBoolean(Helper.GetRandom(0, 1 + 1));

            if (isCurrentEntityPowerup)
            {
                if (Powerup.IsRandomPowerupPositive())
                {
                    powerup = Powerup.GetRandomPositivePowerup();
                }
                else
                {
                    powerup = Powerup.GetRandomNegativePowerup();
                }

                powerup.Spawn();
            }
            else
            {
                obstacle = Obstacle.GetRandomObstacle();

                obstacle.Spawn();
            }

            isEntityAlive = true;
        }

        private void GenerateEntity(float deltaTime)
        {
            if (SpawnCooldownOver() && SpawnChanceAdequate())
            {
                SpawnEntity();

                timeUntilEntitySpawnText.text = $"OBSTACLE/POWERUP: 0.00s";
                timeUntilEntitySpawn = currentEntitySpawnRate;
            }

            if (!isEntityAlive)
            {
                timeUntilEntitySpawn -= Helper.ToSeconds(deltaTime);
                timeUntilEntitySpawnText.text = $"OBSTACLE/POWERUP: {timeUntilEntitySpawn:F}s";
            }
        }

        private bool SpawnCooldownOver()
        {
            return timeUntilEntitySpawn <= 0f && !isEntityAlive;
        }

        private void EntityPhysics(float deltaTime)
        {
            if (isEntityAlive)
            {
                isEntityAlive = currentEntity.Update(deltaTime);
            }
        }

        public void Draw(Graphics gfx)
        {
            timeUntilEntitySpawnText.Draw(gfx);
            PowerupUIManager.instance.Draw(gfx);

            if (isEntityAlive)
            {
                currentEntity.Draw(gfx);
            }
        }

        public void RoundEndLogic()
        {
            isEntityAlive = false;
            timeUntilEntitySpawn = 0f;

            if (currentEntitySpawnRate > HIGHEST_SPAWN_RATE)
            {
                currentEntitySpawnRate -= SPAWN_RATE_INCREASE;
            }

            if (currentObstacleSpawnChance < MAX_SPAWN_CHANCE)
            {
                currentObstacleSpawnChance += SPAWN_CHANCE_INCREASE;
            }
        }

        public void ResetObstacleData()
        {
            isEntityAlive = false;
        }

        public bool IsObstacleImmuneToProjectiles()
        {
            if (!isCurrentEntityPowerup)
            {
                return obstacle.isImmuneToProjectiles;
            }

            return true;
        }
    }
}