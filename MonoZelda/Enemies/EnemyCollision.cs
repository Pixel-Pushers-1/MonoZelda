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
        private readonly int width;
        private readonly int height;
        private IEnemy enemy;
        private Collidable enemyHitbox;
        private CollisionController collisionController;

        public EnemyCollision(IEnemy enemy, CollisionController collisionController)
        {
            this.enemy = enemy;
            this.enemyHitbox = enemy.EnemyHitbox;
            this.width = 64;
            this.height = 64;

            // Create initial hitbox for the player
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

        public void Update()
        {
            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
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
