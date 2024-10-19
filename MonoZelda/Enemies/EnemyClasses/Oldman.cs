using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Oldman : IEnemy
    {
        private Point pos;
        private SpriteDict oldmanSpriteDict;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;
        private double startTime;

        public Oldman(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            oldmanSpriteDict = spriteDict;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);

        }

        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            oldmanSpriteDict.Position = pos;
            oldmanSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    spawning = false;
                    oldmanSpriteDict.SetSprite("oldman");
                }
            }
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            throw new System.NotImplementedException();
        }

        public void DisableProjectile()
        {
        }
    }
}
