
using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class KillStreakEndPowerup : Powerup
    {
        public KillStreakEndPowerup() : base(false, "KillStreakEnd", 1f)
        {
            sprite = new Sprite("assets/powerups/kill_streak_end.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width / 2, sprite.image.Height / 2);
        }

        protected override bool Update(float deltaTime)
        {
            if (IntersectsPlayer())
            {
                Player.instance.ResetKillStreak();

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
