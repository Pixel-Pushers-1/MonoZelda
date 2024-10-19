using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Dodongo : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private readonly SpriteDict dodongoSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime = 0;

        public Dodongo(SpriteDict spriteDict, GraphicsDeviceManager graphics)
        {
            dodongoSpriteDict = spriteDict;
            stateMachine = new CardinalEnemyStateMachine();
            this.graphics = graphics;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
            spawning = true;
        }

        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            dodongoSpriteDict.Position = pos;
            dodongoSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new NotImplementedException();
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    dodongoSpriteDict.SetSprite("dodongo_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    dodongoSpriteDict.SetSprite("dodongo_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    dodongoSpriteDict.SetSprite("dodongo_up");
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    dodongoSpriteDict.SetSprite("dodongo_down");
                    break;
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
                    ChangeDirection();
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
                dodongoSpriteDict.Position = pos;
            }
        }

        public void DisableProjectile()
        {
        }
    }
}
