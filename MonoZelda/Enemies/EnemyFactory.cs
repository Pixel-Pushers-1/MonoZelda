using MonoZelda.Controllers;
using System;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
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

        public Enemy CreateEnemy(EnemyList enemyName, Point spawnPosition)
        {
            var enemyDict = new SpriteDict(SpriteType.Enemies, 0, new Point(0, 0));
            var enemyType = Type.GetType($"MonoZelda.Enemies.EnemyClasses.{enemyName}");
            Enemy enemy = (Enemy)Activator.CreateInstance(enemyType);
            enemy.EnemySpawn(enemyDict, spawnPosition, collisionController);

            return enemy;
        }
    }
}
