using System.Drawing;
using WalmartEngine;

namespace WalmartMario
{
    class FlyingObstacle : Obstacle
    {
        private const int SPAWN_OFFSET_Y = 90;

        public FlyingObstacle() : base(true)
        {
            sprite = new Sprite("assets/obstacles/flying.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width, sprite.image.Height);
        }

        public override void Spawn()
        {
            sprite.truePosition = new Vector2(PlayerCollidableEntityManager.START_POS_X, Player.instance.playerStandDestRecRight + SPAWN_OFFSET_Y);

            base.Spawn();
        }
    }
}