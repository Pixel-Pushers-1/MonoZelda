using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict trapSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private int pixelsMoved;
        public Collidable EnemyHitbox { get; set; }
        private Collidable horizontalHitbox;
        private Collidable verticalHitbox;
        public int Width { get; set; }
        public int Height { get; set; }

        private int tileSize = 64;

        public Trap(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            trapSpriteDict = enemyDict;
            trapSpriteDict.Position = spawnPosition;
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            verticalHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, CollidableType.Enemy);
            horizontalHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, CollidableType.Enemy);
            if (spawnPosition.X < 400)
            {
                if (spawnPosition.Y > 400)
                {
                    horizontalHitbox.Bounds = new Rectangle(tileSize*2, tileSize*11, Width * 12, Height);
                    verticalHitbox.Bounds = new Rectangle(tileSize*2, tileSize * 5, Width, Height * 7);
                }
                else
                {
                    horizontalHitbox.Bounds = new Rectangle(tileSize * 2, tileSize * 5, Width * 12, Height);
                    verticalHitbox.Bounds = new Rectangle(tileSize * 2, tileSize * 5, Width, Height * 7);
                }
            }
            else
            {
                if (spawnPosition.Y > 400)
                {
                    horizontalHitbox.Bounds = new Rectangle(tileSize * 2, tileSize * 11, Width * 12, Height);
                    verticalHitbox.Bounds = new Rectangle(tileSize * 13, tileSize * 5, Width, Height * 7);
                }
                else
                {
                    horizontalHitbox.Bounds = new Rectangle(tileSize * 2, tileSize * 5, Width * 12, Height);
                    verticalHitbox.Bounds = new Rectangle(tileSize * 13, tileSize * 5, Width, Height * 7);
                }
            }
            collisionController.AddCollidable(horizontalHitbox);
            collisionController.AddCollidable(verticalHitbox);
            EnemyHitbox.setSpriteDict(trapSpriteDict); 
            trapSpriteDict.SetSprite("bladetrap");
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine();
        }
        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
