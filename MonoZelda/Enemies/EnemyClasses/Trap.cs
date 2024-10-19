using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.EnemyClasses
{
    public class Trap : IEnemy
    {
        private readonly CardinalEnemyStateMachine stateMachine;
        private Point pos;
        private readonly SpriteDict trapSpriteDict;
        private CardinalEnemyStateMachine.Direction direction;
        private readonly GraphicsDeviceManager graphics;
        private readonly int spawnX;
        private readonly int spawnY;
        private bool spawning;

        private double startTime;
        private readonly CardinalEnemyStateMachine.Direction attackDirection;

        public Trap(SpriteDict spriteDict, GraphicsDeviceManager graphics, CardinalEnemyStateMachine.Direction attackDirection)
        {
            trapSpriteDict = spriteDict;
            stateMachine = new CardinalEnemyStateMachine();
            this.graphics = graphics;
            this.attackDirection = attackDirection;
            direction = attackDirection;
            spawnX = 3 * graphics.PreferredBackBufferWidth / 5;
            spawnY = 3 * graphics.PreferredBackBufferHeight / 5;
            pos = new(spawnX, spawnY);
        }

        public Point Pos { get; set; }
        public Collidable EnemyHitbox { get; set; }

        public void SetOgPos(GameTime gameTime)
        {
            pos.X = spawnX;
            pos.Y = spawnY;
            trapSpriteDict.SetSprite("cloud");
            trapSpriteDict.Position = pos;
            spawning = true;
            startTime = gameTime.TotalGameTime.TotalSeconds;
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController)
        {
            throw new System.NotImplementedException();
        }

        public void EnemySpawn(SpriteDict enemyDict, Point spawnPosition, CollisionController collisionController,
            ContentManager contentManager)
        {
            throw new System.NotImplementedException();
        }

        public void DisableProjectile()
        {
        }

        public void ChangeDirection()
        {
            switch (attackDirection)
            {
                case CardinalEnemyStateMachine.Direction.Left:
                    LeftRight();
                    break;
                case CardinalEnemyStateMachine.Direction.Right:
                    RightLeft();
                    break;
                case CardinalEnemyStateMachine.Direction.Up:
                    UpDown();
                    break;
                case CardinalEnemyStateMachine.Direction.Down:
                    DownUp();
                    break;
            }
            stateMachine.ChangeDirection(direction);
        }

        public void LeftRight()
        {
            if (pos.X <= 0 + 32)
            {
                direction = CardinalEnemyStateMachine.Direction.Right;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.X >= spawnX)
            {
                direction = CardinalEnemyStateMachine.Direction.Left;
                stateMachine.ChangeSpeed(3);
            }

        }

        public void RightLeft()
        {
            if (pos.X >= graphics.PreferredBackBufferWidth - 32)
            {
                direction = CardinalEnemyStateMachine.Direction.Left;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.X <= spawnX)
            {
                direction = CardinalEnemyStateMachine.Direction.Right;
                stateMachine.ChangeSpeed(3);
            }

        }

        public void UpDown()
        {
            if (pos.Y <= 0 + 32)
            {
                direction = CardinalEnemyStateMachine.Direction.Down;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.Y >= spawnY)
            {
                direction = CardinalEnemyStateMachine.Direction.Up;
                stateMachine.ChangeSpeed(3);
            }
        }

        public void DownUp()
        {
            if (pos.Y >= graphics.PreferredBackBufferHeight - 32)
            {
                direction = CardinalEnemyStateMachine.Direction.Up;
                stateMachine.ChangeSpeed(1);
            }
            else if (pos.Y <= spawnY)
            {
                direction = CardinalEnemyStateMachine.Direction.Down;
                stateMachine.ChangeSpeed(3);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (spawning)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= startTime + 0.3)
                {
                    startTime = gameTime.TotalGameTime.TotalSeconds;
                    spawning = false;
                    trapSpriteDict.SetSprite("bladetrap");
                }
            }
            else
            {
                ChangeDirection();
                pos = stateMachine.Update(pos);
                trapSpriteDict.Position = pos;
            }
        }
    }
}
