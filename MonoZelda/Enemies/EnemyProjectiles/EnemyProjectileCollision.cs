using MonoZelda.Collision;
using MonoZelda.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class EnemyProjectileCollision
    {
        private readonly int width;
        private readonly int height;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollidable projectileHitbox;
        private CollisionController collisionController;

        public EnemyProjectileCollision(IEnemyProjectile projectile, CollisionController collisionController)
        {
            this.projectile = projectile;
            projectileHitbox = projectile.ProjectileHitbox;
            width = 32;
            height = 32;

            // Create initial hitbox for the player
            Point projectilePos = projectile.Pos;
            Rectangle bounds = new Rectangle(
                projectilePos.X - width / 2,
                projectilePos.Y - height / 2,
                width,
                height
            );
            this.collisionController = collisionController;

            projectileHitbox.Bounds = bounds;
        }

        public void Update()
        {
            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            Point projectilePos = projectile.Pos;
            Rectangle newBounds = new Rectangle(
                projectilePos.X - width / 2,
                projectilePos.Y - height / 2,
                width,
                height
            );

            projectileHitbox.Bounds = newBounds;
        }
    }
}
