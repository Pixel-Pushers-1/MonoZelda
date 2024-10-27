using System;
using System.Diagnostics.SymbolStore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies.GoriyaFolder;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Goriya : IEnemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private CardinalEnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        private SpriteDict goriyaSpriteDict;
        private CardinalEnemyStateMachine.Direction direction;
        private readonly GraphicsDevice graphicsDevice;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollisionManager projectileCollision;
        private int pixelsMoved;
        private int tilesMoved;
        private int tileSize = 64;
        private bool projectileActive;
        private bool goriyaAlive;
        private int animatedDeath;

        public Goriya(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
            projectileActive = true;
            goriyaAlive = true;
            animatedDeath = 0;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, EnemyList.Goriya);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("cloud");
            goriyaSpriteDict = enemyDict;
            Pos = spawnPosition;
            pixelsMoved = 0;
            tilesMoved = 0;
            stateMachine = new CardinalEnemyStateMachine(enemyDict);
            projectile = new GoriyaBoomerang(spawnPosition, contentManager, graphicsDevice, collisionController);
            projectileCollision = new EnemyProjectileCollisionManager(projectile, collisionController);
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    goriyaSpriteDict.SetSprite("goriya_red_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    goriyaSpriteDict.SetSprite("goriya_red_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    goriyaSpriteDict.SetSprite("goriya_red_up");
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    goriyaSpriteDict.SetSprite("goriya_red_down");
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            projectile.ViewProjectile(projectileActive, goriyaAlive);
            projectile.Update(gameTime, direction, Pos);
            pixelsMoved += 4;
            if (pixelsMoved >= tileSize*6)
            {
                projectile.ViewProjectile(false, goriyaAlive);
                pixelsMoved = 0;
                tilesMoved = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if(goriyaAlive == false)
            {
                if(animatedDeath < 12)
                {
                    goriyaSpriteDict.SetSprite("death");
                    animatedDeath++;
                }
                else
                {
                    TakeDamage(true);
                }
            }
            else if (tilesMoved < 3)
            {
                if (pixelsMoved >= tileSize)
                {
                    pixelsMoved = 0;
                    tilesMoved++;
                    ChangeDirection();
                }

                pixelsMoved++;
                projectile.Follow(Pos);
                Pos = stateMachine.Update(Pos, gameTime);
                goriyaSpriteDict.Position = Pos;
            }
            else
            {
                Attack(gameTime);
            }
            projectileCollision.Update();

        }
        public void TakeDamage(Boolean stun)
        {
            if (goriyaAlive == true && animatedDeath < 12)
            {
                goriyaAlive = false;
                projectileActive = false;
            }
            else if(animatedDeath == 12)
            {
                goriyaSpriteDict.Enabled = false;
                projectile.ViewProjectile(projectileActive, goriyaAlive);
                EnemyHitbox.UnregisterHitbox();
            }
        }
    }
}
