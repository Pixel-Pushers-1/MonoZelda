using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Oldman : Enemy
    {
        public Oldman()
        {
            Width = 64;
            Height = 64;
            Alive = true;

        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            //adjusted position to spawn in middle of screen since we cant with current implementation
            Point adjustedPosition = new Point(spawnPosition.X - 32, spawnPosition.Y + 64);

            EnemyHitbox = new EnemyCollidable(new Rectangle(adjustedPosition.X, adjustedPosition.Y, Width, Height), EnemyList.Oldman);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = adjustedPosition;
            EnemyHitbox.UnregisterHitbox();
            collisionController.RemoveCollidable(EnemyHitbox);
            // Immediately set to oldman sprite instead of cloud
            enemyDict.SetSprite("oldman");
            oldmanSpriteDict = enemyDict;
            Pos = adjustedPosition;
        }

        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (spawnTimer >= 64)
            {
                oldmanSpriteDict.SetSprite("oldman");
            }
            else
            {
                spawnTimer++;
            }
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            // oldman is immortal
        }
    }
}
