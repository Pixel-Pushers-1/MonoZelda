﻿using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework;
using MonoZelda.Link;

namespace MonoZelda.Enemies
{
    public class EnemyCollisionManager
    {
        private int width;
        private int height;
        private EnemyCollidable enemyHitbox;
        private CollisionController collisionController;

        public IEnemy enemy { get; private set; }

        public EnemyCollisionManager(IEnemy enemy, CollisionController collisionController, int width, int height)
        {
            this.enemy = enemy;
            this.enemyHitbox = enemy.EnemyHitbox;
            this.width = width;
            this.height = height;

            Point enemyPosition = enemy.Pos;
            Rectangle bounds = new Rectangle(
                (int)enemyPosition.X - width / 2,
                (int)enemyPosition.Y - height / 2,
                width,
                height
            );
            this.collisionController = collisionController;

            enemyHitbox.Bounds = bounds;
            enemyHitbox.setCollisionManager(this);
        }

        public void Update(int width, int height)
        {
            this.width = width;
            this.height = height;
            UpdateBoundingBox();
        }

        public void UpdateBoundingBox()
        {
            Point enemyPosition = enemy.Pos;
            Rectangle newBounds = new Rectangle(
                (int)enemyPosition.X - width / 2,
                (int)enemyPosition.Y - height / 2,
                width,
                height
            );

            enemyHitbox.Bounds = newBounds;
        }

        public void HandleStaticCollision(Direction collisionDirection, Rectangle intersection)
        {
            Point currentPos = enemy.Pos;
            switch (collisionDirection)
            {
                case Direction.Left:
                    currentPos.X -= intersection.Width;
                    break;
                case Direction.Right:
                    currentPos.X += intersection.Width;
                    break;
                case Direction.Up:
                    currentPos.Y -= intersection.Height;
                    break;
                case Direction.Down:
                    currentPos.Y += intersection.Height;
                    break;
            }
            enemy.ChangeDirection();
            enemy.Pos = currentPos;
        }
    }
}
