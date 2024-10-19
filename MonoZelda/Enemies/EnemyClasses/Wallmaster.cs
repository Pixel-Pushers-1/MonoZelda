using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Wallmaster : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private SpriteDict wallmasterSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;

        public Wallmaster(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            wallmasterSpriteDict = spriteDict;
            stateMachine = new CardinalEnemyStateMachine();
            this.graphics = graphics;
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
            wallmasterSpriteDict.Position = pos;
            wallmasterSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new NotImplementedException();
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            throw new NotImplementedException();
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        //Just using stalfos movement for now since wallmaster moves kind of weird
        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    wallmasterSpriteDict.SetSprite("wallmaster");
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                startTime = gameTime.TotalGameTime.TotalSeconds;
                ChangeDirection();
            }
            else
            {
                pos = stateMachine.Update(pos);
                wallmasterSpriteDict.Position = pos;
            }
        }
    }
}
