﻿using System;
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
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.None;
        private CardinalEnemyStateMachine.Direction projDirection;
        private readonly GraphicsDevice graphicsDevice;
        private IEnemyProjectile projectile;
        private EnemyProjectileCollisionManager projectileCollision;
        private CollisionController collisionController;
        private int pixelsMoved;
        private int tileSize = 64;
        private int health = 3;
        private bool projectileActive;

        public Goriya(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
            Alive = true;
            projectileActive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 48, 48), graphicsDevice, EnemyList.Goriya);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            pixelsMoved = 0;
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
                    stateMachine.SetSprite("goriya_red_left");
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    stateMachine.SetSprite("goriya_red_right");
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    stateMachine.SetSprite("goriya_red_up");
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
                    stateMachine.SetSprite("goriya_red_down");
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Attack(GameTime gameTime)
        {
            projectile.ViewProjectile(projectileActive, Alive);
            projectile.Update(gameTime, projDirection, Pos);
            if (Math.Abs(projectile.Pos.X - Pos.X) < 3 && Math.Abs(projectile.Pos.Y - Pos.Y) < 3)
            {
                projectile.ViewProjectile(false, Alive);
                projDirection = CardinalEnemyStateMachine.Direction.None;
                pixelsMoved = 0;
                ChangeDirection();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (pixelsMoved > tileSize*3 - 1)
            {
                projDirection = direction;
                stateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.None);
                Attack(gameTime);
            }
            else if (pixelsMoved >= 0)
            {
                if (pixelsMoved > 0 && pixelsMoved % tileSize == 0)
                {
                    ChangeDirection();
                }
                projectile.Follow(Pos);
            }
            else
            {
                Attack(gameTime);
            }
            Pos = stateMachine.Update(this, Pos, gameTime);
            projectileCollision.Update();
            pixelsMoved++;
        }
        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            if (stun)
            {
                stateMachine.ChangeDirection(CardinalEnemyStateMachine.Direction.None);
                pixelsMoved = -128;
            }
            else
            {
                health--;
                if (health == 0)
                {
                    projectileActive = false;
                    stateMachine.Die();
                    projectile.ViewProjectile(projectileActive, false);
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
    }
}
