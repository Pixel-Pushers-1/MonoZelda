using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;
using Point = Microsoft.Xna.Framework.Point;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Gel : Enemy
    {
        private double spawnTimer;
        private int jumpCount = 3;
        private bool readyToJump;
        private Random rnd = new Random();

        public Gel()
        {
            Width = 32;
            Height = 32;
            Health = 1;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Gel);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController);
            spawnTimer = 0;
            readyToJump = false;
            StateMachine.SetSprite("gel_turquoise");
        }

        public override void Update()
        {
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
            else if (PixelsMoved >= TileSize * jumpCount)
            {
                Direction = EnemyStateMachine.Direction.None;
                StateMachine.ChangeDirection(Direction);
                PixelsMoved++;
                if (PixelsMoved >= TileSize * jumpCount + 30)
                {
                    readyToJump = true;
                    jumpCount = rnd.Next(1, 4);
                    PixelsMoved = 0;
                }
            }
            else
            {
                PixelsMoved++;
                spawnTimer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            }
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width,Height,Pos);
        }

        public override void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            base.TakeDamage(false, collisionDirection);
        }
    }
}

