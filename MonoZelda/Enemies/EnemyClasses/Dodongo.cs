using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Dodongo : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict dodongoSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private int pixelsMoved;
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }

        private int tileSize = 64;

        public Dodongo(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Dodongo);
            collisionController.AddCollidable(EnemyHitbox);
            dodongoSpriteDict = enemyDict;
            EnemyHitbox.setSpriteDict(dodongoSpriteDict);
            dodongoSpriteDict.Position = spawnPosition;
            dodongoSpriteDict.SetSprite("cloud");
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
                    dodongoSpriteDict.SetSprite("dodongo_left");
                    Width = 96;
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    dodongoSpriteDict.SetSprite("dodongo_right");
                    Width = 96;
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    dodongoSpriteDict.SetSprite("dodongo_up");
                    Width = 64;
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    dodongoSpriteDict.SetSprite("dodongo_down");
                    Width = 64;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

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
                Pos = stateMachine.Update(this, Pos, gameTime);
                dodongoSpriteDict.Position = Pos;
            }
        }

        public void TakeDamage(Boolean stun)
        {
            dodongoSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
