using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class Projectile
    {
        public const float LIFETIME = 1f;

        private static Projectile _instance;

        public bool isAlive { get; private set; }

        public static Projectile instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Projectile();
                }

                return _instance;
            }
        }

        private Sprite sprite;

        private static Vector2 defaultSpeed = new Vector2(3f * 1.8f, -1.8f);

        private Vector2 speed;

        private float timeSinceFired = 0f;

        public Projectile()
        {
            sprite = new Sprite("assets/projectile.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width, sprite.image.Height);

            speed = Vector2.zero;
        }

        public void Fire(Vector2 firePoint)
        {
            sprite.truePosition = firePoint;

            speed = defaultSpeed;

            timeSinceFired = 0f;

            isAlive = true;
        }

        public void Draw(Graphics gfx)
        {
            if (isAlive)
            {
                sprite.Draw(gfx);
            }
        }

        public void Update(float deltaTime)
        {
            if (isAlive)
            {
                if (CollidedWithDamagableObstacle())
                {
                    Player.instance.AddScore();

                    PlayerCollidableEntityManager.instance.ResetObstacleData();

                    Reset();
                }
                else
                {

                    if (IsExpired())
                    {
                        Reset();
                    }
                    else
                    {
                        speed += new Vector2(0f, Physics.GRAVITY);

                        timeSinceFired += Helper.ToSeconds(deltaTime);

                        sprite.truePosition += speed;
                    }
                }
            }
        }

        private bool IsExpired()
        {
            return Ground.IsObjectGrounded(sprite.destRec) || timeSinceFired >= LIFETIME;
        }

        private bool CollidedWithDamagableObstacle()
        {
            PlayerCollidableEntityManager entityManager = PlayerCollidableEntityManager.instance;

            if (entityManager.isEntityAlive &&
!entityManager.isCurrentEntityPowerup &&
!entityManager.IsObstacleImmuneToProjectiles())
            {
                Obstacle obstacle = (Obstacle)entityManager.currentEntity;

                return obstacle.sprite.destRec.IntersectsWith(sprite.destRec);
            }
            else
            {
                return false;
            }
        }

        private void Reset()
        {
            sprite.truePosition = Vector2.zero;
            timeSinceFired = 0f;

            speed = Vector2.zero;

            isAlive = false;
        }

        public void RoundEndLogic()
        {
            Reset();
        }
    }
}