﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Rope : Enemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private EnemyStateMachine stateMachine;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;
        private int pixelsMoved;
        private int health = 3;
        private int tileSize = 64;

        public Rope()
        {
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Rope);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, Width, Height);
            pixelsMoved = 0;
            stateMachine = new EnemyStateMachine(enemyDict, itemFactory, hasItem);
        }

        public override void ChangeDirection()
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

        public override void Update()
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
            Pos = stateMachine.Update(this, Pos);
            enemyCollision.Update(Width, Height, Pos);
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if (stunTime > 0)
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
                    StateMachine.DamageFlash();
                    stateMachine.Knockback(true, collisionDirection);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    stateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }

        public override void LevelOneBehavior()
        {
            throw new NotImplementedException();
        }

        public override void LevelTwoBehavior()
        {
            throw new NotImplementedException();
        }

        public override void LevelThreeBehavior()
        {
            throw new NotImplementedException();
        }

        public override void DecideBehavior()
        {
            throw new NotImplementedException();
        }
    }
}
