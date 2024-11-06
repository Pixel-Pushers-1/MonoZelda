using MonoZelda.Controllers;
using System;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public class EnemyFactory
    {
        private CollisionController collisionController;

        public EnemyFactory(CollisionController collisionController)
        {
            this.collisionController = collisionController;
        }

        public IEnemy CreateEnemy(EnemyList enemyName, Point spawnPosition)
        {
            var enemyDict = new SpriteDict(SpriteType.Enemies, 0, new Point(0, 0));
            var enemyType = Type.GetType($"MonoZelda.Enemies.EnemyClasses.{enemyName}");
            IEnemy enemy = (IEnemy)Activator.CreateInstance(enemyType);
            enemy.EnemySpawn(enemyDict, spawnPosition, collisionController);

            return enemy;
        }
    }
}
