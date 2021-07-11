
using System.Drawing;
using WalmartEngine;
namespace WalmartMario
{
    class SmallObstacle : Obstacle
    {
        public SmallObstacle() : base(true)
        {
            sprite = new Sprite("assets/obstacles/small.png");
            sprite.destRec = new Rectangle(0, 0, sprite.image.Width, sprite.image.Height);
        }

        public override void Spawn()
        {
            sprite.truePosition = new Vector2(PlayerCollidableEntityManager.START_POS_X, Ground.GetGroundedPositionY(sprite.destRec));

            base.Spawn();
        }
    }
}