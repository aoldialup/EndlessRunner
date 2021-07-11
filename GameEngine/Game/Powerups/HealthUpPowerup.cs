
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class HealthUpPowerup : Powerup
    {
        public HealthUpPowerup() : base(true, "HealthUp", 1f)
        {
            sprite = new Sprite("assets/powerups/health_up.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width / 2, sprite.image.Height / 2);
        }

        protected override bool Update(float deltaTime)
        {
            if (IntersectsPlayer())
            {
                Player.instance.AddHealth();

                PowerupUIManager.instance.AddPowerup(this);

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
    }
}
