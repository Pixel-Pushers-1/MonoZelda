using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public class EnemyFactory
    {
        private CollisionController collisionController;
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;

        public EnemyFactory(CollisionController collisionController, ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.collisionController = collisionController;
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
        }

        public IEnemy CreateEnemy(EnemyList enemyName, Point spawnPosition)
        {
            var enemyDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Enemies), SpriteCSVData.Enemies, 0, new Point(0, 0));
            var enemyType = Type.GetType($"MonoZelda.Enemies.EnemyClasses.{enemyName}");
            IEnemy enemy = (IEnemy)Activator.CreateInstance(enemyType, graphicsDevice);
            enemy.EnemySpawn(enemyDict, spawnPosition, collisionController);

            return enemy;
        }
    }
}
