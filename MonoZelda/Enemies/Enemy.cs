using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;
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

        public virtual void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            CollisionController = collisionController;
            EnemyCollision = new EnemyCollisionManager(this, Width, Height);
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

        public virtual void CheckBounds()
        {
            if (Pos.X <= TileSize * 2 + 31 || Pos.X >= TileSize * 14 - 31 || Pos.Y <= TileSize * 5 + 31 || Pos.Y >= TileSize * 12 - 31)
            {
                if (Pos.Y <= TileSize * 5 + 31)
                {
                    var pos = Pos;
                    pos.Y++;
                    Pos = pos;
                } else if (Pos.Y >= TileSize * 12 - 31)
                {
                    var pos = Pos;
                    pos.Y--;
                    Pos = pos;
                }else if (Pos.X <= TileSize * 2 + 31)
                {
                    var pos = Pos;
                    pos.X++;
                    Pos = pos;
                }else if (Pos.X >= TileSize * 14 - 31)
                {
                    var pos = Pos;
                    pos.X--;
                    Pos = pos;
                }
                ChangeDirection();
                PixelsMoved = 0;
            }
        }

        public virtual void Update()
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
            CheckBounds();
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width, Height, Pos);
        }

        public virtual void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if (stunTime > 0)
            {
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                PixelsMoved = (int)(stunTime * TileSize) * - 1;
            }
            else
            {
                Health -= damage;
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
