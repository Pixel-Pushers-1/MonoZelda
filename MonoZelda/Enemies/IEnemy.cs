using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public interface IEnemy
    {
        public Point Pos { get; set; }

        public EnemyCollidable EnemyHitbox { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Boolean Alive { get; set; }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager, Player player);

        public void ChangeDirection();

        public void Update(GameTime gameTime);

        public void TakeDamage(Boolean stun, Direction collisionDirection);
        
    }
}
