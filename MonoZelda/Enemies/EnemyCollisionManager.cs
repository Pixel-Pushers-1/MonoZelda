using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using System.Diagnostics;

namespace MonoZelda.Enemies
{
    public class EnemyCollisionManager
    {
        private int width;
        private int height;
        private Point pos;
        private EnemyCollidable enemyHitbox;

        public Enemy Enemy { get; private set; }

        public EnemyCollisionManager(Enemy enemy, int width, int height)
        {
            this.Enemy = enemy;
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
            Debug.WriteLine($"{enemy} : {enemyHitbox}");
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
            pos = Enemy.Pos;
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
            Enemy.ChangeDirection();
            Enemy.Pos = pos;
        }
    }
}
