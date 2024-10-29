using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework;
using MonoZelda.Link;

namespace MonoZelda.Enemies
{
    public class EnemyCollisionManager
    {
        private int width;
        private int height;
        private Point pos;
        private EnemyCollidable enemyHitbox;
        private CollisionController collisionController;

        public IEnemy enemy { get; private set; }

        public EnemyCollisionManager(IEnemy enemy, CollisionController collisionController, int width, int height)
        {
            this.enemy = enemy;
            this.enemyHitbox = enemy.EnemyHitbox;
            this.width = width;
            this.height = height;

            pos = enemy.Pos;
            Rectangle bounds = new Rectangle(
                pos.X - width / 2,
                pos.X - height / 2,
                width,
                height
            );
            this.collisionController = collisionController;

            enemyHitbox.Bounds = bounds;
            enemyHitbox.setCollisionManager(this);
        }

        public void Update(int width, int height, Point pos)
        {
            this.width = width;
            this.height = height;
            this.pos = pos;
            UpdateBoundingBox();
        }

        public void UpdateBoundingBox()
        {
            Rectangle newBounds = new Rectangle(
                pos.X - width / 2,
                pos.Y - height / 2,
                width,
                height
            );

            enemyHitbox.Bounds = newBounds;
        }

        public void HandleStaticCollision(Direction collisionDirection, Rectangle intersection)
        {
            pos = enemy.Pos;
            switch (collisionDirection)
            {
                case Direction.Left:
                    pos.X -= intersection.Width;
                    break;
                case Direction.Right:
                    pos.X += intersection.Width;
                    break;
                case Direction.Up:
                    pos.Y -= intersection.Height;
                    break;
                case Direction.Down:
                    pos.Y += intersection.Height;
                    break;
            }
            enemy.ChangeDirection();
            enemy.Pos = pos;
        }
    }
}
