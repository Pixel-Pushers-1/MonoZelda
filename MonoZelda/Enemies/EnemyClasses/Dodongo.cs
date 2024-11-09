using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Dodongo : Enemy
    {
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private EnemyStateMachine stateMachine;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;

        private int pixelsMoved;
        private int health = 6;
        private int tileSize = 64;

        public Dodongo()
        {
            Width = 64;
            Height = 64;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,PlayerState player)
        {
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Dodongo);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            Pos = spawnPosition;
            enemyCollision = new EnemyCollisionManager(this, Width, Height);
            pixelsMoved = 0;
            stateMachine = new EnemyStateMachine(enemyDict);
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = EnemyStateMachine.Direction.Left;
                    stateMachine.SetSprite("dodongo_left");
                    Width = 96;
                    break;
                case 2:
                    direction = EnemyStateMachine.Direction.Right;
                    stateMachine.SetSprite("dodongo_right");
                    Width = 96;
                    break;
                case 3:
                    direction = EnemyStateMachine.Direction.Up;
                    stateMachine.SetSprite("dodongo_up");
                    Width = 64;
                    break;
                case 4:
                    direction = EnemyStateMachine.Direction.Down;
                    stateMachine.SetSprite("dodongo_down");
                    Width = 64;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Update()
        {
            if (pixelsMoved >= tileSize)
            {
                pixelsMoved = 0;
                ChangeDirection();
            }
            else
            {
                pixelsMoved++;
                Pos = stateMachine.Update(this, Pos);
            }
            enemyCollision.Update(Width, Height, Pos);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            if (!stun)
            {
                health--;
                if (health == 0)
                {
                    SoundManager.PlaySound("LOZ_Enemy_Hit", false);
                    stateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    collisionController.RemoveCollidable(EnemyHitbox);
                }
                else
                {
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                }
            }
        }
    }
}
