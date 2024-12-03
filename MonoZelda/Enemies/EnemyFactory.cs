using MonoZelda.Controllers;
using System;
using Microsoft.Xna.Framework;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Enemies.EnemyClasses;

namespace MonoZelda.Enemies
{
    public class EnemyFactory
    {
        private CollisionController collisionController;

        public EnemyFactory(CollisionController collisionController)
        {
            this.collisionController = collisionController;
        }

        public Enemy CreateEnemy(EnemyList enemyName, Point spawnPosition, ItemFactory itemFactory, EnemyFactory enemyFactory,bool hasKey)
        {
            var enemyDict = new SpriteDict(SpriteType.Enemies, SpriteLayer.Player + 1, new Point(0, 0));
            var enemyType = Type.GetType($"MonoZelda.Enemies.EnemyClasses.{enemyName}");
            Enemy enemy = (Enemy)Activator.CreateInstance(enemyType);
            if(enemyType == typeof(Gel) && hasKey){
                enemy.Level = 1;
            }
            enemy.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory, enemyFactory, hasKey);

            return enemy;
        }
    }
}
