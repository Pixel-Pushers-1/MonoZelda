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
    public class Dodongo : Enemy
    {
        private readonly Random rnd = new Random();

        private DodongoMouth dodongoMouth;
        private bool ateBomb;
        private double timer;

        public Dodongo()
        {
            Width = 64;
            Height = 64;
            Health = 4;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Dodongo);
            dodongoMouth = new DodongoMouth(this);
            dodongoMouth.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
        }

        public override void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    Direction = EnemyStateMachine.Direction.Left;
                    StateMachine.SetSprite("dodongo_left");
                    Width = 96;
                    break;
                case 2:
                    Direction = EnemyStateMachine.Direction.Right;
                    StateMachine.SetSprite("dodongo_right");
                    Width = 96;
                    break;
                case 3:
                    Direction = EnemyStateMachine.Direction.Up;
                    StateMachine.SetSprite("dodongo_up");
                    Width = 64;
                    break;
                case 4:
                    Direction = EnemyStateMachine.Direction.Down;
                    StateMachine.SetSprite("dodongo_down");
                    Width = 64;
                    break;
            }
            StateMachine.ChangeDirection(Direction);
        }

        public override void Update()
        {
            if(!ateBomb && Health > 0){
                if (PixelsMoved >= TileSize * 3)
                {
                    PixelsMoved = 0;
                    ChangeDirection();
                }
                else
                {
                    PixelsMoved++;
                }
                CheckBounds();
                EnemyCollision.Update(Width,Height,Pos);
                dodongoMouth.Update();
            }else if (ateBomb){
                timer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
                if(timer > 2){
                    string newSprite = Direction switch
                    {
                        EnemyStateMachine.Direction.Left => "dodongo_left",
                        EnemyStateMachine.Direction.Right => "dodongo_right",
                        EnemyStateMachine.Direction.Up => "dodongo_up",
                        EnemyStateMachine.Direction.Down => "dodongo_down",
                        _ => "dodongo_left"
                    };
                    StateMachine.SetSprite(newSprite);
                    StateMachine.ChangeDirection(Direction);
                    SoundManager.PlaySound("LOZ_Boss_Hit", false);
                    Health -= 2;
                    timer = 0;
                    ateBomb = false;
                }else if(timer > 0.5){
                    string newSprite = Direction switch
                    {
                        EnemyStateMachine.Direction.Left => "dodongo_left_hurt",
                        EnemyStateMachine.Direction.Right => "dodongo_right_hurt",
                        EnemyStateMachine.Direction.Up => "dodongo_up_hurt",
                        EnemyStateMachine.Direction.Down => "dodongo_down_hurt",
                        _ => "dodongo_left_hurt"
                    };
                    StateMachine.SetSprite(newSprite);
                }
            }
            if(Health == 0){
                timer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
                StateMachine.DamageFlash();
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                if(timer > 0.5){
                    dodongoMouth.TakeDamage(0, Link.Direction.None, 22);
                    Health--;
                    SoundManager.PlaySound("LOZ_Enemy_Die", false);
                    StateMachine.Die(false);
                    EnemyHitbox.UnregisterHitbox();
                    CollisionController.RemoveCollidable(EnemyHitbox);
                }
            }
            Pos = StateMachine.Update(this, Pos);
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            if(stunTime == 2 && damage == 2 && Health > 0){
                ateBomb = true;
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
            }
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
