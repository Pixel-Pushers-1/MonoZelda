using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Rope : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private readonly SpriteDict ropeSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime;

        public Rope(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            ropeSpriteDict = spriteDict;
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
            ropeSpriteDict.Position = pos;
            ropeSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new NotImplementedException();
        }

        public void ChangeDirection()
        {
            if (pos.X >= graphics.PreferredBackBufferWidth - 32 || pos.X <= 0 + 32)
            {
                switch (rnd.Next(1, 3))
                {
                    case 1:
                        direction = CardinalEnemyStateMachine.Direction.Up;
                        break;
                    case 2:
                        direction = CardinalEnemyStateMachine.Direction.Down;
                        break;
                }
            }
            else if (pos.Y >= graphics.PreferredBackBufferHeight - 32 || pos.Y <= 0 + 32)
            {
                switch (rnd.Next(1, 3))
                {
                    case 1:
                        direction = CardinalEnemyStateMachine.Direction.Left;
                        ropeSpriteDict.SetSprite("rope_left");
                        break;
                    case 2:
                        direction = CardinalEnemyStateMachine.Direction.Right;
                        ropeSpriteDict.SetSprite("rope_right");
                        break;
                }
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    ropeSpriteDict.SetSprite("rope_left");
                    ChangeDirection();
                }
            }
            else
            {
                ChangeDirection();
                pos = stateMachine.Update(pos);
                ropeSpriteDict.Position = pos;
            }
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            throw new NotImplementedException();
        }

        public void DisableProjectile()
        {
        }
    }
}
