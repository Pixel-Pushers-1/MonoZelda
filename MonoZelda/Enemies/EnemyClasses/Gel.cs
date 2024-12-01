using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sprites;
using Point = Microsoft.Xna.Framework.Point;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Gel : Enemy
    {
        private double spawnTimer;
        private int jumpCount = 3;
        private bool readyToJump;
        private Random rnd = new Random();
        private ItemFactory itemFactory;
        private bool zolAlive = false;
        private Enemy bigSlime;
        private List<Enemy> smallSlimes = new();
        private bool smallSlimesAlive = false;

        public Gel()
        {
            Width = 32;
            Height = 32;
            Health = 1;
            Alive = true;
            Level = 3;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            if(Level == 1){
                EnemyHitbox = new EnemyCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, Width, Height), EnemyList.Gel);
                base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
                spawnTimer = 0;
                readyToJump = false;
                StateMachine.SetSprite("gel_turquoise");
                if(hasItem){
                    StateMachine.Spawning = false;
                }
            }
            Pos = spawnPosition;
            this.itemFactory = itemFactory;
            this.EnemyFactory = enemyFactory;

        }

        public override void LevelOneBehavior()
        {
            if (spawnTimer > 1 && spawnTimer < 1.05)
            {
                readyToJump = true;
            }
            if (readyToJump)
            {
                spawnTimer = 1.05;
                ChangeDirection();
                readyToJump = false;
            }
            else if (PixelsMoved >= TileSize * jumpCount)
            {
                Direction = EnemyStateMachine.Direction.None;
                StateMachine.ChangeDirection(Direction);
                PixelsMoved++;
                if (PixelsMoved >= TileSize * jumpCount + 30)
                {
                    readyToJump = true;
                    jumpCount = rnd.Next(1, 4);
                    PixelsMoved = 0;
                }
            }
            else
            {
                PixelsMoved++;
                spawnTimer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            }
            CheckBounds();
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width,Height,Pos);
        }

        public override void LevelTwoBehavior()
        {
            if(!zolAlive){
                bigSlime = EnemyFactory.CreateEnemy(EnemyList.Zol,Pos,itemFactory,EnemyFactory,false);
                zolAlive = true;
            }
            if(bigSlime.Alive){
                bigSlime.Update();
            }else{
                if(!smallSlimesAlive){
                    smallSlimesAlive = true;
                    smallSlimes.Add(EnemyFactory.CreateEnemy(EnemyList.Gel,bigSlime.Pos,itemFactory,EnemyFactory,true));
                }
                updateSlimes();
            }
        }

        public override void LevelThreeBehavior()
        {
            if(!zolAlive){
                bigSlime = EnemyFactory.CreateEnemy(EnemyList.Zol,Pos,itemFactory,EnemyFactory,false);
                zolAlive = true;
            }
            if(bigSlime.Alive){
                bigSlime.Update();
            }else{
                if(!smallSlimesAlive){
                    smallSlimesAlive = true;
                    smallSlimes.Add(EnemyFactory.CreateEnemy(EnemyList.Gel,bigSlime.Pos,itemFactory,EnemyFactory,true));
                    smallSlimes.Add(EnemyFactory.CreateEnemy(EnemyList.Gel,bigSlime.Pos,itemFactory,EnemyFactory,true));
                    smallSlimes.Add(EnemyFactory.CreateEnemy(EnemyList.Gel,bigSlime.Pos,itemFactory,EnemyFactory,true));
                }
                updateSlimes();
            }
        }

        public void updateSlimes(){
            var tempActive = false;
            foreach (var gel in smallSlimes){
                gel.Update();
                if(gel.Alive){
                    tempActive = true;
                }
            }
            if(!tempActive){
                Alive = false;
            }
        }

        public override void Update()
        {
            DecideBehavior();
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            base.TakeDamage(0, collisionDirection, 1);
        }
    }
}

