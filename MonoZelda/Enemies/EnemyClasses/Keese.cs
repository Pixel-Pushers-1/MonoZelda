using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Keese : Enemy
    {
        private readonly Random rnd = new();
        private double speedUpTimer;
        private double dt;
        private float speed;

        public Keese()
        {
            Width = 48;
            Height = 48;
            Health = 1;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Keese);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory, hasItem);
            speed = 0;
            speedUpTimer = 0;
            StateMachine.SetSprite("keese_blue");
        }

        public override void ChangeDirection()
        {
            switch (rnd.Next(1, 9))
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
                case 5:
                    Direction = EnemyStateMachine.Direction.UpLeft;
                    break;
                case 6:
                    Direction = EnemyStateMachine.Direction.UpRight;
                    break;
                case 7:
                    Direction = EnemyStateMachine.Direction.DownLeft;
                    break;
                case 8:
                    Direction = EnemyStateMachine.Direction.DownRight;
                    break;
            }
            StateMachine.ChangeDirection(Direction);
        }

        public override void Update()
        {
            base.Update();
            if (speedUpTimer < 2)
            {
                speedUpTimer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
                speed++;
                StateMachine.ChangeSpeed(speed);
            }
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            base.TakeDamage(0, collisionDirection, 1);
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
