using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies.GoriyaFolder;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Goriya : Enemy
    {
        private EnemyStateMachine.Direction projDirection;
        private List<IEnemyProjectile> boomerangs = new();
        private Dictionary<IEnemyProjectile, EnemyProjectileCollisionManager> projectileDictionary = new();
        private bool projectileActive;

        public Goriya()
        {
            Width = 48;
            Height = 48;
            Health = 3;
            Alive = true;
            projectileActive = false;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Goriya);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
        }

        public override void ChangeDirection()
        {
            base.ChangeDirection();
            string newSprite = Direction switch
            {
                EnemyStateMachine.Direction.Left => "goriya_red_left",
                EnemyStateMachine.Direction.Right => "goriya_red_right",
                EnemyStateMachine.Direction.Up => "goriya_red_up",
                EnemyStateMachine.Direction.Down => "goriya_red_down",
                _ => "goriya_red_left"
            };
            StateMachine.SetSprite(newSprite);
        }
        public override void LevelOneBehavior()
        {
            if (Health > 0)
            {
                if (!projectileActive)
                {
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, projDirection));
                    foreach (var boomerang in boomerangs){
                        projectileDictionary.Add(boomerang,new EnemyProjectileCollisionManager(boomerang));
                    }
                    projectileActive = true;
                }
                boomerangUpdate();
            }
        }

        public override void LevelTwoBehavior()
        {
            if (Health > 0)
            {
                if (!projectileActive)
                {
                    EnemyStateMachine.Direction oppositeDirection = projDirection switch{
                        EnemyStateMachine.Direction.Up => EnemyStateMachine.Direction.Down,
                        EnemyStateMachine.Direction.Down => EnemyStateMachine.Direction.Up,
                        EnemyStateMachine.Direction.Left => EnemyStateMachine.Direction.Right,
                        EnemyStateMachine.Direction.Right => EnemyStateMachine.Direction.Left,
                        _ => EnemyStateMachine.Direction.None
                    };
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, projDirection));
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, oppositeDirection));
                    foreach (var boomerang in boomerangs){
                        projectileDictionary.Add(boomerang,new EnemyProjectileCollisionManager(boomerang));
                    }
                    projectileActive = true;
                }
                boomerangUpdate();
            }
        }

        public override void LevelThreeBehavior()
        {
            if (Health > 0)
            {
                if (!projectileActive)
                {
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, EnemyStateMachine.Direction.Up));
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, EnemyStateMachine.Direction.Down));
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, EnemyStateMachine.Direction.Left));
                    boomerangs.Add(new GoriyaBoomerang(Pos, CollisionController, EnemyStateMachine.Direction.Right));
                    foreach (var boomerang in boomerangs){
                        projectileDictionary.Add(boomerang,new EnemyProjectileCollisionManager(boomerang));
                    }
                    projectileActive = true;
                }
                boomerangUpdate();
            }
        }

        public void boomerangUpdate(){
            var tempActive = false;
            foreach(var boomerang in projectileDictionary){
                    boomerang.Key.ViewProjectile(projectileActive, true);
                    boomerang.Key.Update(projDirection, Pos);
                    boomerang.Value.Update();
                    if (Math.Abs(boomerang.Key.Pos.X - Pos.X) < 16 && Math.Abs(boomerang.Key.Pos.Y - Pos.Y) < 16)
                    {
                        boomerang.Key.ViewProjectile(false, false);
                        boomerangs.Remove(boomerang.Key);
                        projectileDictionary.Remove(boomerang.Key);
                        projDirection = EnemyStateMachine.Direction.None;
                    }else{
                        tempActive = true;
                    }
            }
            if(!tempActive){
                projectileActive = false;
                PixelsMoved = 0;
                ChangeDirection();
            }
        }

        public override void Update()
        {
            if (PixelsMoved > TileSize*3 - 1)
            {
                projDirection = Direction;
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                DecideBehavior();
            }
            else if (PixelsMoved >= 0)
            {
                if (PixelsMoved > 0 && PixelsMoved % TileSize == 0)
                {
                    ChangeDirection();
                }
            }
            else if (projectileActive)
            {
                DecideBehavior();
            }
            CheckBounds();
            Pos = StateMachine.Update(this, Pos);
            PixelsMoved++;
            EnemyCollision.Update(Width,Height,Pos);
        }
        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if (stunTime > 0)
            {
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                PixelsMoved = (int)(stunTime * TileSize) * -1;
            }
            else
            {
                Health -= damage;
                if (Health > 0)
                {
                    SoundManager.PlaySound("LOZ_Enemy_Hit", false);
                    StateMachine.DamageFlash();
                    if (!projectileActive)
                    {
                        StateMachine.Knockback(true, collisionDirection);
                    }
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    StateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    CollisionController.RemoveCollidable(EnemyHitbox);
                    foreach(var boomerang in boomerangs){
                        boomerang.ViewProjectile(false, false);
                    }
                }
            }

        }

    }
}
