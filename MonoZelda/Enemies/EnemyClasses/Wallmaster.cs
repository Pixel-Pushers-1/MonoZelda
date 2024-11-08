using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using System;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Wallmaster : IEnemy
    {
        public Point Pos { get; set; }
        public EnemyCollidable EnemyHitbox { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Alive { get; set; }
        private readonly Random rnd = new();
        private EnemyStateMachine.Direction direction = EnemyStateMachine.Direction.None;
        private EnemyStateMachine stateMachine;
        private CollisionController collisionController;
        private EnemyCollisionManager enemyCollision;
        private PlayerState player;

        public enum PlayerAdjacentWall
        {
            BottomLeft, BottomRight, TopLeft, TopRight, LeftBottom, LeftTop, RightTop, RightBottom, None
        }

        private PlayerAdjacentWall adjacentWall = PlayerAdjacentWall.None;
        private EnemyStateMachine.Direction nextDirection;
        private EnemyStateMachine.Direction returnDirection;
        private Point playerPos;
        private Boolean spawned;
        private float timer;
        private int health = 2;
        private int tileSize = 64;

        public Wallmaster()
        {
            Width = 48;
            Height = 48;
            Alive = true;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, PlayerState player)
        {
            spawned = true;
            timer = (float)(rnd.NextDouble()  - rnd.Next(0,2))*-1;
            this.player = player;
            this.collisionController = collisionController;
            EnemyHitbox = new EnemyCollidable(new Rectangle(-100, -100, Width, Height), EnemyList.Wallmaster);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = new Point(-100, -100);
            Pos = new Point(-100, -100);
            enemyCollision = new EnemyCollisionManager(this, collisionController, Width, Height);
            stateMachine = new EnemyStateMachine(enemyDict);
            stateMachine.Spawning = false;
            stateMachine.SetSprite("wallmaster");
        }
        public void ChangeDirection()
        {
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
                        Pos = new Point(playerPos.X - tileSize*3, tileSize*14);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Up);
                        break;
                    case PlayerAdjacentWall.BottomRight:
                        Pos = new Point(playerPos.X + tileSize * 3, tileSize*14);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Up);
                        break;
                    case PlayerAdjacentWall.TopLeft:
                        Pos = new Point(playerPos.X + tileSize * 3, tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Down);
                        break;
                    case PlayerAdjacentWall.TopRight:
                        Pos = new Point(playerPos.X - tileSize * 3, tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Down);
                        break;
                    case PlayerAdjacentWall.LeftTop:
                        Pos = new Point(0, playerPos.Y - tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Right);
                        break;
                    case PlayerAdjacentWall.LeftBottom:
                        Pos = new Point(0, playerPos.Y + tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Right);
                        break;
                    case PlayerAdjacentWall.RightTop:
                        Pos = new Point(tileSize * 16, playerPos.Y + tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
                        break;
                    case PlayerAdjacentWall.RightBottom:
                        Pos = new Point(tileSize * 16, playerPos.Y - tileSize * 3);
                        stateMachine.ChangeDirection(EnemyStateMachine.Direction.Left);
                        break;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            playerPos = player.Position;
            adjacentWall = PlayerAdjacentWall.None;

            timer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= -0.1 && timer <= -0.01)
            {
                spawned = false;
            }

            if (!spawned)
            {
                //left of bottom wall
                if (playerPos.Y >= tileSize * 11 && playerPos.X <= tileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.BottomLeft;
                    nextDirection = EnemyStateMachine.Direction.Right;
                    returnDirection = EnemyStateMachine.Direction.Down;
                }

                //right of bottom wall
                if (playerPos.Y >= tileSize * 11 && playerPos.X >= tileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.BottomRight;
                    nextDirection = EnemyStateMachine.Direction.Left;
                    returnDirection = EnemyStateMachine.Direction.Down;
                }

                //left of top wall
                if (playerPos.Y <= tileSize * 6 && playerPos.X <= tileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.TopLeft;
                    nextDirection = EnemyStateMachine.Direction.Left;
                    returnDirection = EnemyStateMachine.Direction.Up;
                }

                //right of top wall
                if (playerPos.Y <= tileSize * 6 && playerPos.X >= tileSize * 8)
                {
                    adjacentWall = PlayerAdjacentWall.TopRight;
                    nextDirection = EnemyStateMachine.Direction.Right;
                    returnDirection = EnemyStateMachine.Direction.Up;
                }

                //top of left wall
                if (playerPos.Y <= tileSize * 8 && playerPos.X <= tileSize * 3)
                {
                    adjacentWall = PlayerAdjacentWall.LeftTop;
                    nextDirection = EnemyStateMachine.Direction.Down;
                    returnDirection = EnemyStateMachine.Direction.Left;
                }

                //bottom of left wall
                if (playerPos.Y >= tileSize * 8 && playerPos.X <= tileSize * 3)
                {
                    adjacentWall = PlayerAdjacentWall.LeftBottom;
                    nextDirection = EnemyStateMachine.Direction.Up;
                    returnDirection = EnemyStateMachine.Direction.Left;
                }

                //top of right wall
                if (playerPos.Y <= tileSize * 8 && playerPos.X >= tileSize * 13)
                {
                    adjacentWall = PlayerAdjacentWall.RightTop;
                    nextDirection = EnemyStateMachine.Direction.Up;
                    returnDirection = EnemyStateMachine.Direction.Right;
                }

                //bottom of right wall
                if (playerPos.Y >= tileSize * 8 && playerPos.X >= tileSize * 13)
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
                    stateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                    adjacentWall = PlayerAdjacentWall.None;
                    stateMachine.Die(true);
                    spawned = false;
                }
                else if (timer >= 2.9)
                {
                    stateMachine.ChangeDirection(returnDirection);
                }
                else if (spawned)
                {
                    stateMachine.ChangeDirection(nextDirection);
                }
            }
            Spawn();
            Pos = stateMachine.Update(this, Pos, gameTime);
            enemyCollision.Update(Width, Height, Pos);
        }

        public void TakeDamage(Boolean stun, Direction collisionDirection)
        {
            health--;
            if (health <= 0 && !stun)
            {
                stateMachine.ChangeDirection(EnemyStateMachine.Direction.None);
                stateMachine.Die(true);
                spawned = false;
                health = 2;
            }
        }
    }
}
