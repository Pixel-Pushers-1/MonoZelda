using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Gel : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private readonly Random rnd = new();
        private SpriteDict gelSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private readonly GraphicsDevice graphicsDevice;

        private int tileSize = 64;
        private int spawnTimer;
        private int pixelsMoved;
        private int jumpCount = 3;
        private bool readyToJump;

        public Gel(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Gel);
            collisionController.AddCollidable(EnemyHitbox);
            gelSpriteDict = enemyDict;
            EnemyHitbox.setSpriteDict(gelSpriteDict);
            gelSpriteDict.Position = spawnPosition;
            gelSpriteDict.SetSprite("cloud");
            Pos = spawnPosition;
            pixelsMoved = 0;
            spawnTimer = 0;
            readyToJump = false;
            stateMachine = new CardinalEnemyStateMachine();
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection(direction);
            gelSpriteDict.SetSprite("gel_turquoise");
        }


        public void Update(GameTime gameTime)
        {
            if (spawnTimer > 63 && spawnTimer < 65)
            {
                readyToJump = true;
                Width = 32;
                Height = 32;
            }
            if (readyToJump)
            {
                spawnTimer = 65;
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
                readyToJump = false;
            }
            else if (pixelsMoved >= tileSize * jumpCount)
            {
                direction = CardinalEnemyStateMachine.Direction.None;
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
                spawnTimer++;
                gelSpriteDict.Position = Pos;
            }
            Pos = stateMachine.Update(Pos);
        }

        public void KillEnemy()
        {
            gelSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}

