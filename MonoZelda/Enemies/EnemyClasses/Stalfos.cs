using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Stalfos : IEnemy
    {
        private CardinalEnemyStateMachine stateMachine;
        public Point Pos { get; set; }
        private readonly Random rnd = new();
        private SpriteDict stalfosSpriteDict;
        private CardinalEnemyStateMachine.Direction direction = CardinalEnemyStateMachine.Direction.Left;
        private GraphicsDevice graphicsDevice;
        private bool spawning;
        public Collidable EnemyHitbox { get; set; }

        private double startTime = 0;

        public Stalfos(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController, ContentManager contentManager)
        {
            EnemyHitbox = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Enemy);
            collisionController.AddCollidable(EnemyHitbox);
            EnemyHitbox.setSpriteDict(enemyDict);
            enemyDict.Position = spawnPosition;
            enemyDict.SetSprite("stalfos");
            stalfosSpriteDict = enemyDict;
            Pos = spawnPosition;
            stateMachine = new CardinalEnemyStateMachine();
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            switch (rnd.Next(1, 5))
            {
                case 1:
                    direction = CardinalEnemyStateMachine.Direction.Left;
                    break;
                case 2:
                    direction = CardinalEnemyStateMachine.Direction.Right;
                    break;
                case 3:
                    direction = CardinalEnemyStateMachine.Direction.Up;
                    break;
                case 4:
                    direction = CardinalEnemyStateMachine.Direction.Down;
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
