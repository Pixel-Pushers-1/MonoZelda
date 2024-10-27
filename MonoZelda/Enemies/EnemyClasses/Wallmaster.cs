using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Wallmaster : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict wallmasterSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private int pixelsMoved;
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }

        private int tileSize = 64;

        public Wallmaster(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Wallmaster);
            collisionController.AddCollidable(EnemyHitbox);
            wallmasterSpriteDict = enemyDict;
            EnemyHitbox.setSpriteDict(wallmasterSpriteDict);
            wallmasterSpriteDict.Position = spawnPosition;
            wallmasterSpriteDict.SetSprite("cloud");
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine(enemyDict);
        }
        public void ChangeDirection()
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
            wallmasterSpriteDict.SetSprite("wallmaster");
            stateMachine.ChangeDirection(direction);
        }

        //Just using stalfos movement for now since wallmaster moves kind of weird
        public void Update(GameTime gameTime)
        {
            if (pixelsMoved >= tileSize)
            {
                pixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
                wallmasterSpriteDict.Position = Pos;
            }
            Pos = stateMachine.Update(this, Pos, gameTime);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            wallmasterSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
