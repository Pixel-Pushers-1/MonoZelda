using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyProjectiles
{
    public class EnemyProjectileCollisionManager
    {
        private readonly int width;
        private readonly int height;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollidable projectileHitbox;
        private CollisionController collisionController;

        public IEnemyProjectile Projectile
        {
            get { return projectile; }
            set { projectile = value; }
        }

        public EnemyProjectileCollisionManager(IEnemyProjectile projectile, CollisionController collisionController)
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
            projectileHitbox.setCollisionManager(this);
        }

        public void HandleCollision()
        {
            projectile.ProjectileCollide();
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
