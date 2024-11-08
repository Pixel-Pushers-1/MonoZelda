using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Gel : IEnemy
    {
        private EnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;

        private int tileSize = 64;
        private double spawnTimer;
        private double dt;
        private int pixelsMoved;
        private int jumpCount = 3;
        private bool readyToJump;

        public Gel()
        {
            Width = 32;
            Height = 32;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState Player)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Gel);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            pixelsMoved = 0;
            spawnTimer = 0;
            readyToJump = false;
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("gel_turquoise");
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
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
            }
            stateMachine.ChangeDirection(direction);
        }


        public void Update(GameTime gameTime)
        {
            dt = gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer > 1 && spawnTimer < 1.05)
            {
                readyToJump = true;
            }
            if (readyToJump)
            {
                spawnTimer = 1.05;
                ChangeDirection();
                readyToJump = false;
            }
            else if (pixelsMoved >= tileSize * jumpCount)
            {
                direction = EnemyStateMachine.Direction.None;
                ChangeDirection();
                pixelsMoved++;
                if (pixelsMoved >= tileSize * jumpCount + 30)
                {
                    readyToJump = true;
                    jumpCount = rnd.Next(1, 4);
                    pixelsMoved = 0;
                }
            }
            else
            {
                pixelsMoved++;
                spawnTimer = spawnTimer + dt;
            }
            Pos = stateMachine.Update(this, Pos, gameTime);
            enemyCollision.Update(Width,Height,Pos);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            SoundManager.PlaySound("LOZ_Enemy_Die", false);
            stateMachine.Die(false);
            EnemyHitbox.UnregisterHitbox();
            collisionController.RemoveCollidable(EnemyHitbox);
        }
    }
}

