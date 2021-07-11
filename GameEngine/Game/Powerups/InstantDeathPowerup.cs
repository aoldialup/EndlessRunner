
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class InstantDeathPowerup : Powerup
    {
        public InstantDeathPowerup() : base(false, "InstantDeath", 0f)
        {
            sprite = new Sprite("assets/powerups/instant_death.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width / 2, sprite.image.Height / 2);
        }

        protected override bool Update(float deltaTime)
        {
            if (IntersectsPlayer())
            {
                Player.instance.Kill();

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
