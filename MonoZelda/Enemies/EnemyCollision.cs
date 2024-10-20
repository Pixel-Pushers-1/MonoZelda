using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Numerics;

namespace MonoZelda.Enemies
{
    public class EnemyCollision
    {
        private int width;
        private int height;
        private IEnemy enemy;
        private Collidable enemyHitbox;
        private CollisionController collisionController;

        public EnemyCollision(IEnemy enemy, CollisionController collisionController, int width, int height)
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
    }
}
