
using System;
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class Powerup : IMovable
    {
        protected Sprite sprite;

        protected static Vector2 maxSpeed = new Vector2(-3.5f, 0f);

        protected Vector2 speed;

        public string tag { get; }

        public bool isPositive { get; }

        public float timeUntilShouldRemove { get; protected set; }

        public float maxTimeOnUI { get; }

        public Powerup(bool isPositive, string tag, float maxTimeOnUI)
        {
            this.isPositive = isPositive;
            this.tag = tag;
            this.maxTimeOnUI = maxTimeOnUI;

            timeUntilShouldRemove = maxTimeOnUI;
        }

        protected virtual bool Update(float deltaTime)
        {
            return !PlayerCollidableEntityManager.CONTINUE_UPDATING_ENTITY;
        }

        protected virtual bool IsOutOfBounds()
        {
            return sprite.truePosition.x < Ground.WALL_LEFT_POS_X;
        }

        public virtual void Spawn()
        {
            sprite.truePosition = new Vector2(PlayerCollidableEntityManager.START_POS_X, Ground.GetGroundedPositionY(sprite.destRec));
            speed = maxSpeed;
        }

        public virtual void Draw(Graphics gfx)
        {
            sprite.Draw(gfx);
        }

        protected virtual void Move()
        {
            sprite.truePosition += speed;
        }

        protected virtual bool IntersectsPlayer()
        {
            return sprite.destRec.IntersectsWith(Player.instance.currentState.destRec);
        }

        public static bool IsRandomPowerupPositive()
        {
            return Convert.ToBoolean(Helper.GetRandom(0, 1 + 1)) == true;
        }

        void IMovable.Spawn()
        {
            Spawn();
        }

        void IMovable.Draw(Graphics gfx)
        {
            Draw(gfx);
        }

        bool IMovable.Update(float deltaTime)
        {
            return Update(deltaTime);
        }

        public static Powerup GetRandomPositivePowerup()
        {
            PositivePowerups type = (PositivePowerups)Helper.GetRandom(0, Enum.GetNames(typeof(PositivePowerups)).Length);

            Powerup powerup = null;

            switch (type)
            {
                case PositivePowerups.HEALTH_UP:
                powerup = new HealthUpPowerup();
                break;
            }

            return powerup;
        }

        public static Powerup GetRandomNegativePowerup()
        {
            NegativePowerups type = (NegativePowerups)Helper.GetRandom(0, Enum.GetNames(typeof(NegativePowerups)).Length);

            Powerup powerup = null;

            switch (type)
            {
                case NegativePowerups.HEALTH_DOWN:
                powerup = new HealthDownPowerup();
                break;

                case NegativePowerups.INSTANT_DEATH:
                powerup = new InstantDeathPowerup();
                break;

                case NegativePowerups.KILL_STREAK_END:
                powerup = new KillStreakEndPowerup();
                break;
            }

            return powerup;
        }
    }
}