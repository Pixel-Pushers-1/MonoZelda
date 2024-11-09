using MonoZelda.Controllers;
using System;
using Microsoft.Xna.Framework;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public class EnemyFactory
    {
        private CollisionController collisionController;
        private PlayerState player;

        public EnemyFactory(CollisionController collisionController, PlayerState player)
        {
            this.collisionController = collisionController;
            this.player = player;
        }

        public Enemy CreateEnemy(EnemyList enemyName, Point spawnPosition)
        {
            var enemyDict = new SpriteDict(SpriteType.Enemies, 0, new Point(0, 0));
            var enemyType = Type.GetType($"MonoZelda.Enemies.EnemyClasses.{enemyName}");
            Enemy enemy = (Enemy)Activator.CreateInstance(enemyType);
            enemy.EnemySpawn(enemyDict, spawnPosition, collisionController, player);

            return enemy;
        }
    }
}
