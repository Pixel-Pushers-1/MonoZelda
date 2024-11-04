using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using System.Diagnostics;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : IEnemy
    {
        public Point Pos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine stateMachine;
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private GraphicsDevice graphicsDevice;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;
        private Player player;
        private Boolean attacking;
        private Boolean retreating;

        private int tileSize = 64;

        public Trap(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Width = 64;
            Height = 64;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager, Player player)
        {
            this.collisionController = collisionController;
            enemyDict.Position = spawnPosition;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), graphicsDevice, EnemyList.Trap);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict); 
            Pos = spawnPosition;
            attacking = false;
            retreating = false;
            this.player = player;
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.SetSprite("bladetrap");
        }
        public void ChangeDirection()
        {
        }

        public void tempChangeDirection()
        {
            retreating = false;
            attacking = false;
            direction = EnemyStateMachine.Direction.None;
            stateMachine.ChangeDirection(direction);
            stateMachine.ChangeSpeed(0);
        }

        public void Attack(EnemyStateMachine.Direction attackDirection)
        {
            if (!attacking)
            {
                attacking = true;
                retreating = false;
                stateMachine.ChangeDirection(attackDirection);
                stateMachine.ChangeSpeed(240);
            }
        }

        public void Retreat()
        {
            if (!retreating)
            {
                retreating = true;
                direction = direction switch
                {
                    EnemyStateMachine.Direction.Up => EnemyStateMachine.Direction.Down,
                    EnemyStateMachine.Direction.Down => EnemyStateMachine.Direction.Up,
                    EnemyStateMachine.Direction.Left => EnemyStateMachine.Direction.Right,
                    EnemyStateMachine.Direction.Right => EnemyStateMachine.Direction.Left,
                    _ => EnemyStateMachine.Direction.None
                };
                stateMachine.ChangeDirection(direction);
                stateMachine.ChangeSpeed(120);
            }
        }

        public void CheckBounds()
        {
            if (Pos.X <= tileSize* 2 + 31 || Pos.X >= tileSize * 14 - 31 || Pos.Y <= tileSize*5 + 31 || Pos.Y >= tileSize * 12 - 31)
            {
                if (Pos.Y <= tileSize * 5 + 31)
                {
                    var pos = Pos;
                    pos.Y++;
                    Pos = pos;
                }
                tempChangeDirection();
            }
        }

        public void Update(GameTime gameTime)
        {
            var playerPos = player.GetPlayerPosition().ToPoint();
            if (Math.Abs(playerPos.Y - Pos.Y) < 60 && !attacking)
            {
                if (playerPos.X - Pos.X > 0)
                {
                    direction = EnemyStateMachine.Direction.Right;
                    Attack(direction);
                }
                else
                {
                    direction = EnemyStateMachine.Direction.Left;
                    Attack(direction);
                }
            }
            
            if (Math.Abs(playerPos.X - Pos.X) < 60 && !attacking)
            {
                if (playerPos.Y - Pos.Y > 0)
                {
                    direction = EnemyStateMachine.Direction.Down;
                    Attack(direction);
                }
                else
                {
                    direction = EnemyStateMachine.Direction.Up;
                    Attack(direction);
                }
            }
            
            if (Math.Abs(Pos.X - tileSize * 8) < 32)
            {
                Retreat();
            }
            
            if (Math.Abs(Pos.Y - (int)(tileSize * 8.5)) < 32)
            {
                Retreat();
            }

            Pos = stateMachine.Update(this, Pos, gameTime);
            enemyCollision.Update(Width, Height, Pos);
            CheckBounds();
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            //cannot take damage
        }
    }
}
