using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.EnemyProjectiles;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Aquamentus : IEnemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private EnemyStateMachine stateMachine;
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.Left;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;

        private List<IEnemyProjectile> fireballs = new();
        private Dictionary<IEnemyProjectile, EnemyProjectileCollisionManager> projectileDictionary = new();
        private int health = 6;
        private Point spawnPoint;
        private int pixelsMoved;
        private int tileSize = 64;
        private int moveDelay;
        private double attackDelay;
        private double dt;
        private bool projectileActive;
        private PlayerState player;

        public Aquamentus()
        {
            projectileActive = false;
            pixelsMoved = 0;
            moveDelay = rnd.Next(1, 4);
            attackDelay = 0;
            Width = 32;
            Height = 84;
            Alive = true;

        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState player)
        {
            this.player = player;
            this.collisionController = collisionController;
            spawnPoint = spawnPosition;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Aquamentus);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            pixelsMoved = 0;
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("aquamentus_left");
            stateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
            stateMachine.Spawning = false;
            stateMachine.ChangeSpeed(60);
        }

        public void ChangeDirection()
        {
            switch (direction)
            {
                case EnemyStateMachine.Direction.Left:
                    direction = EnemyStateMachine.Direction.Right;
                    stateMachine.ChangeDirection(direction);
                    break;
                case EnemyStateMachine.Direction.Right:
                    direction = EnemyStateMachine.Direction.Left;
                    stateMachine.ChangeDirection(direction);
                    break;
            }
        }

        public void Attack(GameTime gameTime)
        {
            fireballs.ForEach(fireball => fireball.Update(gameTime, EnemyStateMachine.Direction.Left, Pos));
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
                attackDelay = 0;
            }
        }

        public void CreateFireballs()
        {
            var move = player.Position.ToVector2() - Pos.ToVector2();
            move = Vector2.Divide(move, (float)Math.Sqrt(move.X * move.X + move.Y * move.Y)) * 6;
            if (!projectileActive)
            {
                projectileActive = true;
                fireballs.Add(new AquamentusFireball(Pos, collisionController, new Vector2(move.X,move.Y - 2)));
                fireballs.Add(new AquamentusFireball(Pos, collisionController, move));
                fireballs.Add(new AquamentusFireball(Pos, collisionController, new Vector2(move.X, move.Y + 2)));
                foreach (var projectile in fireballs)
                {
                    projectileDictionary.Add(projectile, new EnemyProjectileCollisionManager(projectile, collisionController));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            dt = gameTime.ElapsedGameTime.TotalSeconds;
            Pos = stateMachine.Update(this, Pos, gameTime);
            pixelsMoved++;
            if (Pos.X > spawnPoint.X + 10 || Pos.X < spawnPoint.X - tileSize*5 - 10)
            {
                ChangeDirection();
                pixelsMoved = 0;
            }
            else
            {
                if (pixelsMoved >= tileSize*moveDelay)
                {
                    pixelsMoved = 0;
                    ChangeDirection();
                    moveDelay = rnd.Next(1, 5);
                }
            }

            if (fireballs.Count == 0 && health > 0)
            {
                SoundManager.PlaySound("LOZ_Boss_Scream1", false);
                CreateFireballs();
                stateMachine.SetSprite("aquamentus_left_mouthopen");
            }
            else
            {
                stateMachine.SetSprite("aquamentus_left");
                Attack(gameTime);
            }
            foreach (KeyValuePair<IEnemyProjectile, EnemyProjectileCollisionManager> entry in projectileDictionary)
            {
                entry.Value.Update();
            }
            enemyCollision.Update(Width, Height, new Point(Pos.X - 16, Pos.Y - 16));
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            if (!stun)
            {
                health--;
                if (health == 0)
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    fireballs.ForEach(fireball => fireball.ProjectileCollide());
                    stateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Boss_Hit", false);
                }
            }
        }
    }
}
