﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Rope : IEnemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private EnemyStateMachine stateMachine;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;

        private int pixelsMoved;
        private int health = 3;
        private int tileSize = 64;

        public Rope(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Rope);
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
            stateMachine.SetSprite("rope_left");
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = EnemyStateMachine.Direction.Left;
                    stateMachine.SetSprite("rope_left");
                    break;
                case 2:
                    direction = EnemyStateMachine.Direction.Right;
                    stateMachine.SetSprite("rope_right");
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
            Pos = stateMachine.Update(this, Pos, gameTime);
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
