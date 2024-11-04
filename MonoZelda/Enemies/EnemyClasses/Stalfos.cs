using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : IEnemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Boolean Alive { get; set; }
        private EnemyStateMachine stateMachine;
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private readonly GraphicsDevice graphicsDevice;
        private CollisionController collisionController;
        private int pixelsMoved;
        private int health = 2;
        private readonly int tileSize = 64;
        private readonly Random rnd = new();
        private EnemyCollisionManager enemyCollision;

        public Stalfos(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Stalfos);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            pixelsMoved = 0;
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("stalfos");
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
            if (pixelsMoved >= tileSize)
            {
                pixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
            }
            Pos = stateMachine.Update(this,Pos,gameTime);
            enemyCollision.Update(Width, Height, Pos);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            if (stun)
            {
                stateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                pixelsMoved = -128;
            }
            else
            {
                health--;
                if (health > 0)
                {
                    stateMachine.Knockback(true, collisionDirection);
                }
                else
                {
                    stateMachine.Die();
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
    }
}
