using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Zol : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private readonly SpriteDict zolSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;
        private bool readyToJump = true;

        public Zol(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            zolSpriteDict = spriteDict;
            stateMachine = new CardinalEnemyStateMachine();
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
            zolSpriteDict.Position = pos;
            zolSpriteDict.SetSprite("cloud");
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
            stateMachine.ChangeDirection(direction);
        }


        public void Update(GameTime gameTime) //too long
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    readyToJump = true;
                    zolSpriteDict.SetSprite("zol_green");
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

                startTime = gameTime.TotalGameTime.TotalSeconds;
                readyToJump = false;
                ChangeDirection();
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                direction = CardinalEnemyStateMachine.Direction.None;
                ChangeDirection();
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 2)
                {
                    readyToJump = true;
                }
            }
            else
            {
                pos = stateMachine.Update(pos);
                zolSpriteDict.Position = pos;
            }
        }
    }
}
