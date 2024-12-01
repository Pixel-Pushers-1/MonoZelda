using System;
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
        private IEnemyProjectile projectile;
        private EnemyProjectileCollisionManager projectileCollision;
        private bool projectileActive;

        public Goriya()
        {
            Width = 48;
            Height = 48;
            Health = 3;
            Alive = true;
            projectileActive = false;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Goriya);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory, hasItem);
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
            };
            StateMachine.SetSprite(newSprite);
        }

        public void Attack()
        {
            if (Health > 0)
            {
                if (!projectileActive)
                {
                    projectile = new GoriyaBoomerang(Pos, CollisionController, projDirection);
                    projectileCollision = new EnemyProjectileCollisionManager(projectile);
                    projectileActive = true;
                }
                projectile.ViewProjectile(projectileActive, true);
                projectile.Update(projDirection, Pos);
                projectileCollision.Update();
                if (Math.Abs(projectile.Pos.X - Pos.X) < 16 && Math.Abs(projectile.Pos.Y - Pos.Y) < 16)
                {
                    projectileActive = false;
                    projectile.ViewProjectile(projectileActive, false);
                    projectile = null;
                    projectileCollision = null;
                    projDirection = EnemyStateMachine.Direction.None;
                    PixelsMoved = 0;
                    ChangeDirection();
                }
            }
        }

        public override void Update()
        {
            if (PixelsMoved > TileSize*3 - 1)
            {
                projDirection = Direction;
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                Attack();
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
                Attack();
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
                    if (projectile != null)
                    {
                        projectile.ViewProjectile(false, false);
                    }
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
