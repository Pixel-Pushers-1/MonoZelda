﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Zol : IEnemy
    {
        private EnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private CollisionController collisionController;

        private int health = 2;
        private int tileSize = 64;
        private int pixelsMoved;
        private bool readyToJump;

        public Zol()
        {
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Zol);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            pixelsMoved = 0;
            readyToJump = false;
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("zol_brown");
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
            if (readyToJump)
            {
                readyToJump = false;
                ChangeDirection();
            }
            else if (pixelsMoved >= tileSize)
            {
                direction = EnemyStateMachine.Direction.None;
                ChangeDirection();
                pixelsMoved++;
                if (pixelsMoved >= tileSize + 30)
                {
                    pixelsMoved = 0;
                    readyToJump = true;
                }
            }
            else
            {
                pixelsMoved++;
                Pos = stateMachine.Update(this, Pos, gameTime);
            }
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
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    stateMachine.Die();
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
    }
}
