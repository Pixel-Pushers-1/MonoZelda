﻿using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Aquamentus : Enemy
    {
        private readonly Random rnd = new();

        private List<IEnemyProjectile> fireballs = new();
        private Dictionary<IEnemyProjectile, EnemyProjectileCollisionManager> projectileDictionary = new();
        private Point spawnPoint;
        private int moveDelay;
        private bool projectileActive;

        public Aquamentus()
        {
            projectileActive = false;
            moveDelay = rnd.Next(1, 4);
            Width = 32;
            Height = 84;
            Health = 6;
            Alive = true;

        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Aquamentus);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            spawnPoint = spawnPosition;
            StateMachine.SetSprite("aquamentus_left");
            StateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
            StateMachine.Spawning = false;
            StateMachine.ChangeSpeed(60);
        }

        public override void ChangeDirection()
        {
            switch (Direction)
            {
                case EnemyStateMachine.Direction.Left:
                    Direction = EnemyStateMachine.Direction.Right;
                    StateMachine.ChangeDirection(Direction);
                    break;
                case EnemyStateMachine.Direction.Right:
                    Direction = EnemyStateMachine.Direction.Left;
                    StateMachine.ChangeDirection(Direction);
                    break;
            }
        }

        public void Attack()
        {
            fireballs.ForEach(fireball => fireball.Update( EnemyStateMachine.Direction.Left, Pos));
            var tempActive = false;
            foreach (var entry in projectileDictionary)
            {
                if (entry.Key.Active)
                {
                    tempActive = true;
                }

                if (!entry.Key.Active)
                {
                    fireballs.Remove(entry.Key);
                    projectileDictionary.Remove(entry.Key);
                }
            }

            if (!tempActive)
            {
                projectileActive = false;
            }
        }

        public override void LevelOneBehavior()
        {
            var move = PlayerState.Position.ToVector2() - Pos.ToVector2();
            move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
            if (!projectileActive)
            {
                projectileActive = true;
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X,move.Y - 2)));
                fireballs.Add(new Fireball(Pos, CollisionController, move));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X, move.Y + 2)));
                foreach (var projectile in fireballs)
                {
                    projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile));
                }
            }
        }

        public override void LevelTwoBehavior()
        {
            var move = PlayerState.Position.ToVector2() - Pos.ToVector2();
            move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
            if (!projectileActive)
            {
                projectileActive = true;
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y - 1)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1, move.Y + 1)));
                foreach (var projectile in fireballs)
                {
                    projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile));
                }
            }
        }

        public override void LevelThreeBehavior()
        {
            var move = PlayerState.Position.ToVector2() - Pos.ToVector2();
            move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
            if (!projectileActive)
            {
                projectileActive = true;
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y - 2)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y - 1)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y + 1)));
                fireballs.Add(new Fireball(Pos, CollisionController, new Vector2(move.X - 1,move.Y + 2)));
                foreach (var projectile in fireballs)
                {
                    projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile));
                }
            }
        }

        public override void Update()
        {
            Pos = StateMachine.Update(this, Pos);
            PixelsMoved++;
            if (Pos.X > spawnPoint.X + 10 || Pos.X < spawnPoint.X - TileSize*5 - 10)
            {
                ChangeDirection();
                PixelsMoved = 0;
            }
            else
            {
                if (PixelsMoved >= TileSize*moveDelay)
                {
                    PixelsMoved = 0;
                    ChangeDirection();
                    moveDelay = rnd.Next(1, 5);
                }
            }

            if (fireballs.Count == 0 && Health > 0)
            {
                SoundManager.PlaySound("LOZ_Boss_Scream1", false);
                DecideBehavior();
                StateMachine.SetSprite("aquamentus_left_mouthopen");
            }
            else
            {
                StateMachine.SetSprite("aquamentus_left");
                Attack();
            }
            foreach (KeyValuePair<IEnemyProjectile, EnemyProjectileCollisionManager> entry in projectileDictionary)
            {
                entry.Value.Update();
            }
            CheckBounds();
            EnemyCollision.Update(Width, Height, new Point(Pos.X - 16, Pos.Y - 16));
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if (stunTime == 0)
            {
                Health -= damage;
                if (Health <= 0)
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    fireballs.ForEach(fireball => fireball.ProjectileCollide());
                    StateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    CollisionController.RemoveCollidable(EnemyHitbox);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Boss_Hit", false);
                    StateMachine.DamageFlash();
                }
            }
        }

    }
}
