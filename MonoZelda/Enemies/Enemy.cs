using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using Direction = MonoZelda.Link.Direction;

namespace MonoZelda.Enemies
{
    public abstract class Enemy
    {
        public const int TileSize = 64;
        private const int LeftBound = TileSize * 2 + 31;
        private const int RightBound = TileSize * 14 - 31;
        private const int TopBound = TileSize * 5 + 31;
        private const int BottomBound = TileSize * 12 - 31;

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
        public float Timer {get; set; }
        public int Level {get; set; }
        public EnemyFactory EnemyFactory {get; set;}

        public virtual void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, EnemyFactory enemyFactory, bool hasItem)
        {
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            CollisionController = collisionController;
            EnemyCollision = new EnemyCollisionManager(this, Width, Height);
            StateMachine = new EnemyStateMachine(enemyDict, itemFactory, hasItem);
            EnemyFactory = enemyFactory;
            Level = MonoZeldaGame.EnemyLevel;
            Timer = 0;
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

        public virtual int CheckBounds()
        {
            var boundHit = 0;
            if (Pos.X <= LeftBound || Pos.X >= RightBound || Pos.Y <= TopBound || Pos.Y >= BottomBound)
            {
                if (Pos.Y <= TopBound)
                {
                    var pos = Pos;
                    pos.Y++;
                    Pos = pos;
                    boundHit = TopBound;
                } else if (Pos.Y >= BottomBound)
                {
                    var pos = Pos;
                    pos.Y--;
                    Pos = pos;
                    boundHit = BottomBound;
                }else if (Pos.X <= LeftBound)
                {
                    var pos = Pos;
                    pos.X++;
                    Pos = pos;
                    boundHit = LeftBound;
                }else if (Pos.X >= RightBound)
                {
                    var pos = Pos;
                    pos.X--;
                    Pos = pos;
                    boundHit = RightBound;
                }

                StateMachine.TakingKnockback = false;
                ChangeDirection();
            }
            return boundHit;
        }

        public abstract void LevelOneBehavior();
        public abstract void LevelTwoBehavior();
        public abstract void LevelThreeBehavior();

        public virtual void DecideBehavior(){
            switch (Level){
                case 1:
                    LevelOneBehavior();
                    break;
                case 2:
                    LevelTwoBehavior();
                    break;
                case 3:
                    LevelThreeBehavior();
                    break;
            }
        }

        public virtual void Update()
        {
            Timer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            if (PixelsMoved >= TileSize)
            {
                PixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                PixelsMoved++;
            }
            DecideBehavior();
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
                    StateMachine.DamageFlash();
                    StateMachine.Knockback(true, collisionDirection);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    StateMachine?.Die(false);
                    EnemyHitbox?.UnregisterHitbox();
                    CollisionController?.RemoveCollidable(EnemyHitbox);
                }
            }
        }
        
    }
}
