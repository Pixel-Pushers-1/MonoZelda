using System;
using System.Diagnostics.SymbolStore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies.GoriyaFolder;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Link;
using MonoZelda.Sound;
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
        private EnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private EnemyStateMachine.Direction projDirection;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollisionManager projectileCollision;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;
        private int pixelsMoved;
        private int tileSize = 64;
        private int health = 3;
        private bool projectileActive;

        public Goriya()
        {
            Width = 48;
            Height = 48;
            Alive = true;
            projectileActive = false;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState player)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Goriya);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            pixelsMoved = 0;
            stateMachine = new EnemyStateMachine(enemyDict);
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = EnemyStateMachine.Direction.Left;
                    stateMachine.SetSprite("goriya_red_left");
                    break;
                case 2:
                    direction = EnemyStateMachine.Direction.Right;
                    stateMachine.SetSprite("goriya_red_right");
                    break;
                case 3:
                    direction = EnemyStateMachine.Direction.Up;
                    stateMachine.SetSprite("goriya_red_up");
                    break;
                case 4:
                    direction = EnemyStateMachine.Direction.Down;
                    stateMachine.SetSprite("goriya_red_down");
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            if (!projectileActive && health > 0)
            {
                projectile = new GoriyaBoomerang(Pos, collisionController, projDirection);
                projectileCollision = new EnemyProjectileCollisionManager(projectile, collisionController);
                projectileActive = true;
            }
            projectile.ViewProjectile(projectileActive, true);
            projectile.Update(gameTime, projDirection, Pos);
            projectileCollision.Update();
            if (Math.Abs(projectile.Pos.X - Pos.X) < 16 && Math.Abs(projectile.Pos.Y - Pos.Y) < 16)
            {
                projectileActive = false;
                projectile.ViewProjectile(projectileActive, false);
                projectile = null;
                projectileCollision = null;
                projDirection = EnemyStateMachine.Direction.None;
                pixelsMoved = 0;
                ChangeDirection();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (pixelsMoved > tileSize*3 - 1)
            {
                projDirection = direction;
                stateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                Attack(gameTime);
            }
            else if (pixelsMoved >= 0)
            {
                if (pixelsMoved > 0 && pixelsMoved % tileSize == 0)
                {
                    ChangeDirection();
                }
            }
            else if (projectileActive)
            {
                Attack(gameTime);
            }

            Pos = stateMachine.Update(this, Pos, gameTime);
            pixelsMoved++;
            enemyCollision.Update(Width,Height,Pos);
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
                    SoundManager.PlaySound("LOZ_Enemy_Hit", false);
                    stateMachine.Knockback(true, collisionDirection);
                }
                else
                {
                    if (projectile != null)
                    {
                        projectile.ViewProjectile(false, false);
                    }

                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    projectileActive = false;
                    stateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
    }
}
