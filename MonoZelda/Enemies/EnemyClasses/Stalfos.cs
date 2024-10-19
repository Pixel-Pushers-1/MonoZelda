using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Enemies.StalfosFolder;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : IEnemy
    {
        private StalfosStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict stalfosSpriteDict;
        private StalfosStateMachine.Direction direction = StalfosStateMachine.Direction.Left;
        private GraphicsDevice graphicsDevice;
        private bool spawning;
        public Collidable EnemyHitbox { get; set; }

        private double startTime = 0;

        public Stalfos(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void SetOgPos(GameTime gameTime)
        {
            stalfosSpriteDict.SetSprite("cloud");
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Item);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("stalfos");
            stalfosSpriteDict = enemyDict;
            Pos = spawnPosition;
            stateMachine = new StalfosStateMachine();
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = StalfosStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = StalfosStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = StalfosStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = StalfosStateMachine.Direction.Down;
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    stalfosSpriteDict.SetSprite("stalfos");
                }
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= startTime + 1)
            {
                startTime = gameTime.TotalGameTime.TotalSeconds;
                ChangeDirection();
            }
            else
            {
                Pos = stateMachine.Update(Pos);
                stalfosSpriteDict.Position = Pos;
            }
        }
    }
}
