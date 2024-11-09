using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using MonoZelda.Link;
using Direction = MonoZelda.Link.Direction;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : Enemy
    {
        private readonly Random rnd = new();
        private PlayerState player;
        private Boolean attacking;
        private Boolean retreating;

        public Trap()
        {
            Width = 64;
            Height = 64;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState player)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Trap);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, player);
            attacking = false;
            retreating = false;
            this.player = player;
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

        public void CheckBounds()
        {
            if (Pos.X <= TileSize* 2 + 31 || Pos.X >= TileSize * 14 - 31 || Pos.Y <= TileSize*5 + 31 || Pos.Y >= TileSize * 12 - 31)
            {
                if (Pos.Y <= TileSize * 5 + 31)
                {
                    var pos = Pos;
                    pos.Y++;
                    Pos = pos;
                }
                ChangeDirection();
            }
        }

        public override void Update(GameTime gameTime)
        {
            var playerPos = player.Position;
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

            Pos = StateMachine.Update(this, Pos, gameTime);
            EnemyCollision.Update(Width, Height, Pos);
            CheckBounds();
        }

        public override void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            //cannot take damage
        }
    }
}
