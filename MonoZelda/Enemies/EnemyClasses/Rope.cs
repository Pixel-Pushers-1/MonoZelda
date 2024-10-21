using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Rope : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict ropeSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private int pixelsMoved;
        public Collidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private int tileSize = 64;

        public Rope(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            ropeSpriteDict = enemyDict;
            EnemyHitbox.setSpriteDict(ropeSpriteDict);
            ropeSpriteDict.Position = spawnPosition;
            ropeSpriteDict.SetSprite("cloud");
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine();
            EnemyHitbox.setEnemy(this);
        }

        public void ChangeDirection()
        {
            ropeSpriteDict.SetSprite("rope_left");
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    ropeSpriteDict.SetSprite("rope_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    ropeSpriteDict.SetSprite("rope_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
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
                ropeSpriteDict.Position = Pos;
            }
            Pos = stateMachine.Update(Pos);
        }

        public void KillEnemy()
        {
            ropeSpriteDict.Enabled = false;
            EnemyHitbox.UnregisterHitbox();
        }
    }
}
