using System;
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
        private SpriteDict stalfosSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private int pixelsMoved;
        public Collidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private int tileSize = 64;
        private bool stalfosAlive;
        private int animatedDeath;

        public Stalfos(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
            stalfosAlive = true;
            animatedDeath = 0;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            stalfosSpriteDict = enemyDict;
            Pos = spawnPosition;
            pixelsMoved = 0;
            stateMachine = new CardinalEnemyStateMachine();
            EnemyHitbox.setEnemy(this);
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
            stalfosSpriteDict.SetSprite("stalfos");
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            if (stalfosAlive == false)
            {
                if (animatedDeath < 12)
                {
                    stalfosSpriteDict.SetSprite("death");
                    animatedDeath++;
                }
                else
                {
                    KillEnemy();
                }
            }
            else if (pixelsMoved >= tileSize)
            {
                pixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
                stalfosSpriteDict.Position = Pos;
            }
            Pos = stateMachine.Update(Pos);
        }

        public void KillEnemy()
        {
            if (stalfosAlive == true && animatedDeath < 12)
            {
                stalfosAlive = false;
            }
            else if (animatedDeath == 12)
            {
                stalfosSpriteDict.Enabled = false;
                EnemyHitbox.UnregisterHitbox();
            }
        }
    }
}
