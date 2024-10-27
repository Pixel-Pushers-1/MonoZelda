using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies
{
    public class CardinalEnemyStateMachine
    {
        //states
        public enum Direction { Left, Right, Up, Down, None }
        private Direction direction = Direction.None;
        private bool spawning;
        public bool Dead { get; set; }

        //not states
        private float velocity = 60;
        private float dt;
        private double animationTimer;
        private SpriteDict enemySpriteDict;
        private string currentSprite;
        private Vector2 movement;
        public Point currentPosition { get; private set; }

        public CardinalEnemyStateMachine(SpriteDict spriteDict)
        {
            enemySpriteDict = spriteDict;
            spawning = true;
            Dead = false;
            animationTimer = 0;
            enemySpriteDict.SetSprite("cloud");
        }
      
        public void ChangeDirection(Direction newDirection)
        {
            direction = newDirection;
        }

        public void SetSprite(string newSprite)
        {
            currentSprite = newSprite;
        }

        public void ChangeSpeed(int newSpeed)
        {
            velocity = newSpeed;
        }

        public void Die()
        {
            Dead = true;
            animationTimer = 0;
            enemySpriteDict.SetSprite("death");
        }

        public Point Update(Point position, GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 enemyPosition = position.ToVector2();
            if (spawning)
            {
                animationTimer = animationTimer + dt;
                if (animationTimer >= 1)
                {
                    spawning = false;
                }
            }
            else if (Dead)
            {
                animationTimer = animationTimer + dt;
                if (animationTimer >= 0.5)
                {
                    enemySpriteDict.Enabled = false;
                }
            }
            else
            {
                movement = direction switch
                {
                    Direction.Up => new Vector2(0, -1),
                    Direction.Down => new Vector2(0, 1),
                    Direction.Left => new Vector2(-1, 0),
                    Direction.Right => new Vector2(1, 0),
                    _ => Vector2.Zero
                };
                enemyPosition += (velocity * movement)*dt;
                enemySpriteDict.SetSprite(currentSprite);
            }

            position = enemyPosition.ToPoint();
            enemySpriteDict.Position = position;
            currentPosition = position;
            return position;
        }
    }
}
