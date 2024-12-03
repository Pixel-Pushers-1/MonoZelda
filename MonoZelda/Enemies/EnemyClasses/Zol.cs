using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Zol : Enemy
    {
        private readonly Random rnd = new();
        private bool readyToJump;

        public Zol()
        {
            Width = 48;
            Height = 48;
            Health = 2;
            Alive = true;
        }

        public override void DecideBehavior()
        {
            throw new NotImplementedException();
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Zol);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            readyToJump = false;
            switch (MonoZeldaGame.EnemyLevel){
                    case 1:
                        StateMachine.SetSprite("zol_turquoise");
                        break;
                    case 2:
                        StateMachine.SetSprite("zol_turquoise");
                        break;
                    case 3:
                        StateMachine.SetSprite("zol_black");
                        break;
                }
        }

        public override void LevelOneBehavior()
        {
            throw new NotImplementedException();
        }

        public override void LevelThreeBehavior()
        {
            throw new NotImplementedException();
        }

        public override void LevelTwoBehavior()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if (readyToJump)
            {
                readyToJump = false;
                ChangeDirection();
            }
            else if (PixelsMoved >= TileSize)
            {
                Direction = EnemyStateMachine.Direction.None;
                StateMachine.ChangeDirection(Direction);
                PixelsMoved ++;
                if (PixelsMoved >= TileSize + 30)
                {
                    PixelsMoved = 0;
                    readyToJump = true;
                }
            }
            else
            {
                PixelsMoved += 2;
                Pos = StateMachine.Update(this, Pos);
            }
            CheckBounds();
            EnemyCollision.Update(Width, Height, Pos);
        }
    }
}
