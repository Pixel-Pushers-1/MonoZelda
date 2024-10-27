using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private CollisionController collisionController;
        private int pixelsMoved;
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Boolean Alive { get; set; }
        private int health = 2;
        private int tileSize = 64;
        private double damageDelay;
        private double dt;

        public Stalfos(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 48, 48), graphicsDevice, EnemyList.Stalfos);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine(enemyDict);
            stateMachine.SetSprite("stalfos");
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
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            dt = gameTime.ElapsedGameTime.TotalSeconds;
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
            damageDelay -= dt;
        }

        public void TakeDamage(Boolean stun)
        {
            if (stun)
            {
                stateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.None);
                pixelsMoved = -128;
            }
            else
            {
                health--;
                damageDelay = 0.5;
                if (health == 0)
                {
                    stateMachine.Die();
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
    }
}
