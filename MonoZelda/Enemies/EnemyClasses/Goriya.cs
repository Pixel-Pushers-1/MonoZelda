using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.GoriyaFolder;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Goriya : IEnemy
    {
        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }
        private CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly Random rnd = new();
        private SpriteDict goriyaSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private readonly GraphicsDevice graphicsDevice;
        private bool spawning;

        private GoriyaBoomerang boomerang;

        private double startTime;
        private double attackDelay;

        public Goriya(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            boomerang.BoomerangSpriteDict.Enabled = false;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Item);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("goriya_red_left");
            goriyaSpriteDict = enemyDict;
            Pos = spawnPosition;
            stateMachine = new CardinalEnemyStateMachine();
        }

        public void DisableProjectile()
        {
            boomerang.BoomerangSpriteDict.Enabled = false;
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    goriyaSpriteDict.SetSprite("goriya_red_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    goriyaSpriteDict.SetSprite("goriya_red_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    goriyaSpriteDict.SetSprite("goriya_red_up");
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    goriyaSpriteDict.SetSprite("goriya_red_down");
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            boomerang.BoomerangSpriteDict.Enabled = true;
            boomerang.Update(gameTime, direction, attackDelay);
            if (gameTime.TotalGameTime.TotalSeconds >= attackDelay + 5)
            {
                attackDelay = gameTime.TotalGameTime.TotalSeconds;
                boomerang.BoomerangSpriteDict.Enabled = false;
            }
        }

        public void Update(GameTime gameTime) //might eventually split this into multiple methods, controlling random movement is more extensive than I thought.
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds < attackDelay + 3)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
                {
                    ChangeDirection();
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                }
                boomerang.Follow(pos);
                pos = stateMachine.Update(pos);
                goriyaSpriteDict.Position = pos;
            }
            else
            {
                Attack(gameTime);
            }
        }
    }
}
