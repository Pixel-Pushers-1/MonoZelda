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
    public class Zol : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private SpriteDict zolSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private readonly GraphicsDevice graphicsDevice;

        private int tileSize = 64;
        private int pixelsMoved;
        private bool readyToJump;

        public Zol(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Zol);
            collisionController.AddCollidable(EnemyHitbox);
            zolSpriteDict = enemyDict;
            EnemyHitbox.setSpriteDict(zolSpriteDict);
            zolSpriteDict.Position = spawnPosition;
            zolSpriteDict.SetSprite("cloud");
            Pos = spawnPosition;
            pixelsMoved = 0;
            readyToJump = false;
            stateMachine = new CardinalEnemyStateMachine(enemyDict);
        }
        public void ChangeDirection()
        {
            stateMachine.ChangeDirection(direction);
            zolSpriteDict.SetSprite("zol_brown");
        }


        public void Update(GameTime gameTime)
        {
            if (readyToJump)
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
                readyToJump = false;
                ChangeDirection();
            }
            else if (pixelsMoved >= tileSize)
            {
                direction = CardinalEnemyStateMachine.Direction.None;
                ChangeDirection();
                pixelsMoved++;
                if (pixelsMoved >= tileSize + 30)
                {
                    pixelsMoved = 0;
                    readyToJump = true;
                }
            }
            else
            {
                pixelsMoved++;
                Pos = stateMachine.Update(this, Pos, gameTime);
                zolSpriteDict.Position = Pos;
            }
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            zolSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
