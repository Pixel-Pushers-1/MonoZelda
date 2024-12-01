using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Commands;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Wallmaster : Enemy
    {
        public enum PlayerAdjacentWall
        {
            BottomLeft, BottomRight, TopLeft, TopRight, LeftBottom, LeftTop, RightTop, RightBottom, None
        }
        private readonly Random rnd = new();
        private Boolean grabbed;
        private CommandManager commandManager;
        private PlayerAdjacentWall adjacentWall = PlayerAdjacentWall.None;
        private EnemyStateMachine.Direction nextDirection;
        private EnemyStateMachine.Direction returnDirection;
        private Point playerPos;
        private Boolean spawned;
        private float timer;

        public Wallmaster()
        {
            Width = 48;
            Height = 48;
            Health = 2;
            Alive = true;
        }

        public override void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ItemFactory itemFactory,EnemyFactory enemyFactory, bool hasItem)
        {
            EnemyHitbox = new EnemyCollidable(new Rectangle(-100, -100, Width, Height), EnemyList.Wallmaster);
            base.EnemySpawn(enemyDict, spawnPosition, collisionController, itemFactory,enemyFactory, hasItem);
            spawned = true;
            timer = (float)(rnd.NextDouble()  - rnd.Next(0,2))*-1;
            Pos = new Point(-100, -100);
            StateMachine.Spawning = false;
            grabbed = false;
            StateMachine.SetSprite("wallmaster");
        }

        public void GrabPlayer(CommandManager commandManager)
        {
            this.commandManager = commandManager;
            grabbed = true;
            timer = 2.9f;
        }

        public void Spawn()
        {
            if (!spawned && adjacentWall != PlayerAdjacentWall.None)
            {
                timer = 0;
                spawned = true;
                switch (adjacentWall)
                {
                    case PlayerAdjacentWall.BottomLeft:
                        Pos = new Point(playerPos.X - TileSize*3, TileSize*14);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Up);
                        break;
                    case PlayerAdjacentWall.BottomRight:
                        Pos = new Point(playerPos.X + TileSize * 3, TileSize*14);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Up);
                        break;
                    case PlayerAdjacentWall.TopLeft:
                        Pos = new Point(playerPos.X + TileSize * 3, TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Down);
                        break;
                    case PlayerAdjacentWall.TopRight:
                        Pos = new Point(playerPos.X - TileSize * 3, TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Down);
                        break;
                    case PlayerAdjacentWall.LeftTop:
                        Pos = new Point(0, playerPos.Y - TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Right);
                        break;
                    case PlayerAdjacentWall.LeftBottom:
                        Pos = new Point(0, playerPos.Y + TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Right);
                        break;
                    case PlayerAdjacentWall.RightTop:
                        Pos = new Point(TileSize * 16, playerPos.Y + TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
                        break;
                    case PlayerAdjacentWall.RightBottom:
                        Pos = new Point(TileSize * 16, playerPos.Y - TileSize * 3);
                        StateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
                        break;
                }
            }
        }

        public override void Update()
        {
            playerPos = PlayerState.Position;
            adjacentWall = PlayerAdjacentWall.None;
            if (spawned)
            {
                timer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            }

            if (timer >= -0.1 && timer <= -0.01)
            {
                spawned = false;
            }

            if (!spawned)
            {
                //left of bottom wall
                if (playerPos.Y >= TileSize * 11 && playerPos.X <= TileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.BottomLeft;
                    nextDirection = EnemyStateMachine.Direction.Right;
                    returnDirection = EnemyStateMachine.Direction.Down;
                }

                //right of bottom wall
                if (playerPos.Y >= TileSize * 11 && playerPos.X >= TileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.BottomRight;
                    nextDirection = EnemyStateMachine.Direction.Left;
                    returnDirection = EnemyStateMachine.Direction.Down;
                }

                //left of top wall
                if (playerPos.Y <= TileSize * 6 && playerPos.X <= TileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.TopLeft;
                    nextDirection = EnemyStateMachine.Direction.Left;
                    returnDirection = EnemyStateMachine.Direction.Up;
                }

                //right of top wall
                if (playerPos.Y <= TileSize * 6 && playerPos.X >= TileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.TopRight;
                    nextDirection = EnemyStateMachine.Direction.Right;
                    returnDirection = EnemyStateMachine.Direction.Up;
                }

                //top of left wall
                if (playerPos.Y <= TileSize * 8 && playerPos.X <= TileSize * 3)
                {
                    adjacentWall = PlayerAdjacentWall.LeftTop;
                    nextDirection = EnemyStateMachine.Direction.Down;
                    returnDirection = EnemyStateMachine.Direction.Left;
                }

                //bottom of left wall
                if (playerPos.Y >= TileSize * 8 && playerPos.X <= TileSize * 3)
                {
                    adjacentWall = PlayerAdjacentWall.LeftBottom;
                    nextDirection = EnemyStateMachine.Direction.Up;
                    returnDirection = EnemyStateMachine.Direction.Left;
                }

                //top of right wall
                if (playerPos.Y <= TileSize * 8 && playerPos.X >= TileSize * 13)
                {
                    adjacentWall = PlayerAdjacentWall.RightTop;
                    nextDirection = EnemyStateMachine.Direction.Up;
                    returnDirection = EnemyStateMachine.Direction.Right;
                }

                //bottom of right wall
                if (playerPos.Y >= TileSize * 8 && playerPos.X >= TileSize * 13)
                {
                    adjacentWall = PlayerAdjacentWall.RightBottom;
                    nextDirection = EnemyStateMachine.Direction.Down;
                    returnDirection = EnemyStateMachine.Direction.Right;
                }

                timer = 0;

            }

            if (timer >= 1.30)
            {
                if (timer >= 4.2)
                {
                    StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                    adjacentWall = PlayerAdjacentWall.None;
                    StateMachine.Die(true);
                    spawned = false;
                    if (grabbed)
                    {
                        //might need to use a different type of transition here, idk what it is in game
                        commandManager.Execute(CommandType.RoomTransitionCommand, "Room1", Link.Direction.Down);
                    }
                }
                else if (timer >= 2.9)
                {
                    StateMachine.ChangeDirection(returnDirection);
                    // if link is grabbed animation should start here
                }
                else if (spawned)
                {
                    StateMachine.ChangeDirection(nextDirection);
                }
            }
            Spawn();
            Pos = StateMachine.Update(this, Pos);
            EnemyCollision.Update(Width, Height, Pos);
        }

        public override void TakeDamage(float stunTime, Direction collisionDirection, int damage)
        {
            Health -= damage;
            StateMachine.DamageFlash();
            if (Health <= 0 && stunTime == 0)
            {
                StateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                SoundManager.PlaySound("LOZ_Enemy_Die", false);
                StateMachine.Die(true);
                spawned = false;
                Health = 2;
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
