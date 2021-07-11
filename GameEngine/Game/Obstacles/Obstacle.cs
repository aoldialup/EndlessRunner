using System;
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class Obstacle : IMovable
    {
        public const int KILL_SCORE = 10;
        public const int MIN_KILLS_NEEDED = 5;

        protected static Vector2 maxSpeed = new Vector2(-3f, 0f);

        public bool isImmuneToProjectiles { get; }

        public Sprite sprite { get; protected set; }

        protected Vector2 speed;

        public Obstacle(bool isImmuneToProjectiles)
        {
            this.isImmuneToProjectiles = isImmuneToProjectiles;
        }

        protected virtual bool IntersectsPlayer()
        {
            return sprite.destRec.IntersectsWith(Player.instance.currentState.destRec);
        }

        protected virtual void Move()
        {
            sprite.truePosition += speed;
        }

        protected virtual bool IsOutOfBounds()
        {
            return sprite.truePosition.x < Ground.WALL_LEFT_POS_X;
        }

        public virtual void Spawn()
        {
            speed = maxSpeed;
        }

        protected virtual bool Update(float deltaTime)
        {
            if (IntersectsPlayer())
            {
                Player.instance.TakeDamage();

                return !PlayerCollidableEntityManager.CONTINUE_UPDATING_ENTITY;
            }
            else if (IsOutOfBounds())
            {
                return !PlayerCollidableEntityManager.CONTINUE_UPDATING_ENTITY;
            }
            else
            {
                Move();
            }

            return PlayerCollidableEntityManager.CONTINUE_UPDATING_ENTITY;
        }

        protected virtual void Draw(Graphics gfx)
        {
            sprite.Draw(gfx);
        }

        void IMovable.Spawn()
        {
            Spawn();
        }

        bool IMovable.Update(float deltaTime)
        {
            return Update(deltaTime);
        }

        void IMovable.Draw(Graphics gfx)
        {
            Draw(gfx);
        }

        public static Obstacle GetRandomObstacle()
        {
            Obstacles obstacleType = (Obstacles)Helper.GetRandom(0, Enum.GetNames(typeof(Obstacles)).Length);

            Obstacle obstacle = null;

            switch (obstacleType)
            {
                case Obstacles.FLYING:
                obstacle = new FlyingObstacle();
                break;

                case Obstacles.SMALL:
                obstacle = new SmallObstacle();
                break;

                case Obstacles.TALL:
                obstacle = new TallObstacle();
                break;
            }

            return obstacle;
        }
    }
}