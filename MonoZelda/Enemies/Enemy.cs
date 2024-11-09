using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using static MonoZelda.Enemies.EnemyStateMachine;
using Direction = MonoZelda.Link.Direction;

namespace MonoZelda.Enemies
{
    public abstract class Enemy
    {
        public const int TileSize = 64;

        public Point Pos { get; set; }

        public EnemyCollidable EnemyHitbox { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Boolean Alive { get; set; }
        public CollisionController CollisionController { get; set; }
        public EnemyCollisionManager EnemyCollision { get; set; }
        public int PixelsMoved { get; set; }
        public int Health { get; set; }
        public EnemyStateMachine StateMachine { get; set; }
        public EnemyStateMachine.Direction Direction { get; set; }
        private readonly Random rnd = new Random();

        public virtual void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            PlayerState player)
        {
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            CollisionController = collisionController;
            EnemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            StateMachine = new EnemyStateMachine(enemyDict);
        }

        public virtual void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    Direction = EnemyStateMachine.Direction.Left;
                    break;
                case 2:
                    Direction = EnemyStateMachine.Direction.Right;
                    break;
                case 3:
                    Direction = EnemyStateMachine.Direction.Up;
                    break;
                case 4:
                    Direction = EnemyStateMachine.Direction.Down;
                    break;
            }
            StateMachine.ChangeDirection(Direction);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (PixelsMoved >= TileSize)
            {
                PixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                PixelsMoved++;
            }
            Pos = StateMachine.Update(this, Pos, gameTime);
            EnemyCollision.Update(Width, Height, Pos);
        }

        public virtual void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            if (stun)
            {
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                PixelsMoved = -128;
            }
            else
            {
                Health--;
                if (Health > 0)
                {
                    SoundManager.PlaySound("LOZ_Enemy_Hit", false);
                    StateMachine.Knockback(true, collisionDirection);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    StateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    CollisionController.RemoveCollidable(EnemyHitbox);
                }
            }
        }
        
    }
}
