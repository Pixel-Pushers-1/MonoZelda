using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public interface IEnemy
    {
        public Point Pos { get; set; }

        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime);

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController);

        public void DisableProjectile();

        public void ChangeDirection();

        public void Update(GameTime gameTime);
        
    }
}
