using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : Enemy
    {
        public Stalfos()
        {
            Width = 48;
            Height = 48;
            Health = 2;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Stalfos);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController);
            StateMachine.SetSprite("stalfos");
        }
    }
}
