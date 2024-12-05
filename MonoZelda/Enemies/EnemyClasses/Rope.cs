using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Rope : Enemy
    {
        private const int LeftBound = TileSize * 2 + 31;
        private const int RightBound = TileSize * 14 - 31;
        private const int TopBound = TileSize * 5 + 31;
        private const int BottomBound = TileSize * 12 - 31;
        private bool attacking = false;
        private int boundHit;
        private readonly Random rnd = new Random();

        public Rope()
        {
            Width = 48;
            Height = 48;
            Health = 2;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Rope);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            base.ChangeDirection();
            ChangeSprite();
        }

        public override void ChangeDirection()
        {
            if(boundHit == TopBound || boundHit == BottomBound){
                Direction = rnd.Next(1, 3) switch
                {
                    1 => EnemyStateMachine.Direction.Left,
                    2 => EnemyStateMachine.Direction.Right,
                    _ => EnemyStateMachine.Direction.Left
                };
            }else if(boundHit == LeftBound || boundHit == RightBound){
                Direction = rnd.Next(1, 3) switch
                {
                    1 => EnemyStateMachine.Direction.Up,
                    2 => EnemyStateMachine.Direction.Down,
                    _ => EnemyStateMachine.Direction.Up
                };
            }else{
                base.ChangeDirection();
            }
            StateMachine.ChangeDirection(Direction);
            StateMachine.ChangeSpeed(120);
            ChangeSprite();
        }

        public void ChangeSprite(){
            string newSprite = Direction switch
            {
                EnemyStateMachine.Direction.Left => "rope_left",
                EnemyStateMachine.Direction.Right => "rope_right",
                _ => "rope_left"
            };
            StateMachine.SetSprite(newSprite);
        }

        public override void LevelOneBehavior()
        {
            if (!attacking)
                {
                    attacking = true;
                    StateMachine.ChangeDirection(Direction);
                    ChangeSprite();
                    StateMachine.ChangeSpeed(240);
                }
        }
        public override void LevelTwoBehavior()
        {
            throw new NotImplementedException();
        }

        public override void LevelThreeBehavior()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            var playerPos = PlayerState.Position;
            boundHit = CheckBounds();
            if(boundHit != 0){
                attacking = false;
                ChangeDirection();
            }
            if (Math.Abs(playerPos.Y - Pos.Y) < 25 && !attacking)
            {
                if (playerPos.X - Pos.X > 0)
                {
                    Direction = EnemyStateMachine.Direction.Right;
                    LevelOneBehavior();
                }
                else
                {
                    Direction = EnemyStateMachine.Direction.Left;
                    LevelOneBehavior();
                }
            }
            
            if (Math.Abs(playerPos.X - Pos.X) < 25 && !attacking)
            {
                if (playerPos.Y - Pos.Y > 0)
                {
                    Direction = EnemyStateMachine.Direction.Down;
                    LevelOneBehavior();
                }
                else
                {
                    Direction = EnemyStateMachine.Direction.Up;
                    LevelOneBehavior();
                }
            }
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width, Height, Pos);
        }
    }
}
