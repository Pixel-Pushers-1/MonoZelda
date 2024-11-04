using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : IEnemy
    {
        public Point Pos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine stateMachine;
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;

        private int tileSize = 64;

        public Trap(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            this.collisionController = collisionController;
            enemyDict.Position = spawnPosition;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Trap);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict); 
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("bladetrap");
        }
        public void ChangeDirection()
        {
        }

        public void Update(GameTime gameTime)
        {
            enemyCollision.Update(Width, Height, Pos);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            //cannot take damage
        }
    }
}
