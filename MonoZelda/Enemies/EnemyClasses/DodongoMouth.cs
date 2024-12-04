using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Items;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class DodongoMouth : Enemy
    {
        private readonly Random rnd = new Random();
        private Enemy dodongo;
        private SpriteDict mouthDict;

        public DodongoMouth(Enemy enemy)
        {
            dodongo = enemy;
            Width = 64;
            Height = 64;
            Health = 1;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.DodongoMouth);
            mouthDict = new SpriteDict(SpriteType.Enemies,0,spawnPosition);
            base.EnemySpawn(mouthDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            StateMachine.SetSprite("gel_black");
        }

        public override void ChangeDirection()
        {
            switch (dodongo.Direction)
            {
                case EnemyStateMachine.Direction.Left:
                    Direction = EnemyStateMachine.Direction.Left;
                    Width = 96;
                    Height = 64;
                    break;
                case EnemyStateMachine.Direction.Right:
                    Direction = EnemyStateMachine.Direction.Right;
                    Width = 96;
                    Height = 64;
                    break;
                case EnemyStateMachine.Direction.Up:
                    Direction = EnemyStateMachine.Direction.Up;
                    Height = 96;
                    Width = 64;
                    break;
                case EnemyStateMachine.Direction.Down:
                    Direction = EnemyStateMachine.Direction.Down;
                    Height = 96;
                    Width = 64;
                    break;
            }
            StateMachine.ChangeDirection(Direction);
        }

        public void UpdateMouthCollision(){
            var newPos = Direction switch{
                EnemyStateMachine.Direction.Left => new Point(Pos.X - 64, Pos.Y),
                EnemyStateMachine.Direction.Right => new Point(Pos.X + 64, Pos.Y),
                EnemyStateMachine.Direction.Up => new Point(Pos.X, Pos.Y - 64),
                EnemyStateMachine.Direction.Down => new Point(Pos.X, Pos.Y + 64),
                _ => Pos
            };
            EnemyCollision.Update(Width, Height, newPos);
        }

        public override void Update()
        {
            ChangeDirection();
            Pos = StateMachine.Update(this, dodongo.Pos);
            UpdateMouthCollision();
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if(damage == 22){
                Alive = false;
                mouthDict.Unregister();
                EnemyHitbox.UnregisterHitbox();
                CollisionController.RemoveCollidable(EnemyHitbox);
                
            }
            dodongo.TakeDamage(stunTime, collisionDirection, damage);
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
    }
}
