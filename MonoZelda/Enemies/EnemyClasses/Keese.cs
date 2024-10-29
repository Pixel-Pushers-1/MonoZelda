using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Keese : IEnemy
    {
        private EnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private readonly GraphicsDevice graphicsDevice;
        private CollisionController collisionController;
        private int pixelsMoved;
        private double speedUpTimer;
        private double dt;
        private float speed;

        public Keese(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Keese);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            Pos = spawnPosition;
            pixelsMoved = 0;
            speed = 0;
            speedUpTimer = 0;
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("keese_blue");
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 9))
            {
                case 1:
                    direction = EnemyStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = EnemyStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = EnemyStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = EnemyStateMachine.Direction.Down;
                    break;
                case 5:
                    direction = EnemyStateMachine.Direction.UpLeft;
                    break;
                case 6:
                    direction = EnemyStateMachine.Direction.UpRight;
                    break;
                case 7:
                    direction = EnemyStateMachine.Direction.DownLeft;
                    break;
                case 8:
                    direction = EnemyStateMachine.Direction.DownRight;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            dt = gameTime.ElapsedGameTime.TotalSeconds;
            if (speedUpTimer < 2)
            {
                speedUpTimer += dt;
                speed++;
                stateMachine.ChangeSpeed(speed);
            }
            if (pixelsMoved >= 64)
            {
                pixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
            }
            Pos = stateMachine.Update(this, Pos, gameTime);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            stateMachine.Die();
            EnemyHitbox.UnregisterHitbox();
            collisionController.RemoveCollidable(EnemyHitbox);
        }
    }
}
