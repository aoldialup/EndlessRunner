
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class HealthDownPowerup : Powerup
    {
        public HealthDownPowerup() : base(false, "HealthDown", 1f)
        {
            sprite = new Sprite("assets/powerups/health_down.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width / 2, sprite.image.Height / 2);
        }

        protected override bool Update(float deltaTime)
        {
            if (IntersectsPlayer())
            {
                Player.instance.TakeDamage();

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