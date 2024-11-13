using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Items;
using MonoZelda.Link;
using Direction = MonoZelda.Link.Direction;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : Enemy
    {
        private readonly Random rnd = new();
        private Boolean attacking;
        private Boolean retreating;

        public Trap()
        {
            Width = 64;
            Height = 64;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Trap);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory, hasItem);
            attacking = false;
            retreating = false;
            StateMachine.SetSprite("bladetrap");
            StateMachine.Spawning = false;
        }
        public override void ChangeDirection()
        {
            retreating = false;
            attacking = false;
            Direction = EnemyStateMachine.Direction.None;
            StateMachine.ChangeDirection(Direction);
            StateMachine.ChangeSpeed(0);
        }

        public void Attack(EnemyStateMachine.Direction attackDirection)
        {
            if (!attacking)
            {
                attacking = true;
                retreating = false;
                StateMachine.ChangeDirection(attackDirection);
                StateMachine.ChangeSpeed(240);
            }
        }

        public void Retreat()
        {
            if (!retreating)
            {
                retreating = true;
                Direction = Direction switch
                {
                    EnemyStateMachine.Direction.Up => EnemyStateMachine.Direction.Down,
                    EnemyStateMachine.Direction.Down => EnemyStateMachine.Direction.Up,
                    EnemyStateMachine.Direction.Left => EnemyStateMachine.Direction.Right,
                    EnemyStateMachine.Direction.Right => EnemyStateMachine.Direction.Left,
                    _ => EnemyStateMachine.Direction.None
                };
                StateMachine.ChangeDirection(Direction);
                StateMachine.ChangeSpeed(120);
            }
        }

        public override void Update()
        {
            var playerPos = PlayerState.Position;
            if (Math.Abs(playerPos.Y - Pos.Y) < 60 && !attacking)
            {
                if (playerPos.X - Pos.X > 0)
                {
                    Direction = EnemyStateMachine.Direction.Right;
                    Attack(Direction);
                }
                else
                {
                    Direction = EnemyStateMachine.Direction.Left;
                    Attack(Direction);
                }
            }
            
            if (Math.Abs(playerPos.X - Pos.X) < 60 && !attacking)
            {
                if (playerPos.Y - Pos.Y > 0)
                {
                    Direction = EnemyStateMachine.Direction.Down;
                    Attack(Direction);
                }
                else
                {
                    Direction = EnemyStateMachine.Direction.Up;
                    Attack(Direction);
                }
            }
            
            if (Math.Abs(Pos.X - TileSize * 8) < 32)
            {
                Retreat();
            }
            
            if (Math.Abs(Pos.Y - (int)(TileSize * 8.5)) < 32)
            {
                Retreat();
            }

            CheckBounds();
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width, Height, Pos);
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            //cannot take damage
        }
    }
}
