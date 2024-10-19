using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Gel : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos; // will change later
        private readonly Random rnd = new();
        private readonly SpriteDict gelSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;
        private int jumpCount;
        private bool readyToJump = true;

        public Gel(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            gelSpriteDict = spriteDict;
            stateMachine = new CardinalEnemyStateMachine();
            jumpCount = rnd.Next(1, 4);
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
        }


        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime) //sets to spawn position (eventually could be used for re-entering rooms)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            gelSpriteDict.Position = pos;
            gelSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new NotImplementedException();
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection(direction);
        }


        public void Update(GameTime gameTime) //too long
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    readyToJump = true;
                    spawning = false;
                    gelSpriteDict.SetSprite("gel_turquoise");
                }
            }
            else if (readyToJump)
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
                ChangeDirection();
                startTime = gameTime.TotalGameTime.TotalSeconds;
                readyToJump = false;
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + jumpCount)
            {
                direction = CardinalEnemyStateMachine.Direction.None;
                ChangeDirection();
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + jumpCount + 0.75)
                {
                    readyToJump = true;
                    jumpCount = rnd.Next(1, 4);
                }
            }
            else
            {
                pos = stateMachine.Update(pos);
                gelSpriteDict.Position = pos;
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
