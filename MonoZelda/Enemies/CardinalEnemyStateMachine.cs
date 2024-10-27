using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Link;

namespace MonoZelda.Enemies
{
    public class CardinalEnemyStateMachine
    {
        //states
        public enum Direction { Left, Right, Up, Down, None }
        private Direction direction = Direction.None;
        private bool spawning;
        public bool Dead { get; set; }
        private bool knockback = false;

        //not states
        private const float KNOCKBACK_FORCE = 1000f;
        private float velocity = 60f;
        private float dt;
        private double animationTimer;
        private float knockbackTimer;
        private Vector2 knockbackDirection;
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

        public void ChangeSpeed(float newSpeed)
        {
            velocity = newSpeed;
        }

        public void Knockback(bool takesKnockback, Link.Direction collisionDirection)
        {
            if (takesKnockback)
            {
                knockback = true;
                knockbackDirection = collisionDirection switch
                {
                    Link.Direction.Up => new Vector2(0, 1),
                    Link.Direction.Down => new Vector2(0, -1),
                    Link.Direction.Left => new Vector2(1, 0),
                    Link.Direction.Right => new Vector2(-1, 0),
                    _ => Vector2.Zero
                };
            }
        }

        public void Die()
        {
            Dead = true;
            animationTimer = 0;
            enemySpriteDict.SetSprite("death");
        }

        public Point Update(IEnemy enemy, Point position, GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 enemyPosition = position.ToVector2();
            if (knockback)
            {
                knockbackTimer += dt;
                if (knockbackTimer >= 0.05)
                {
                    velocity = 60;
                    knockback = false;
                    knockbackTimer = 0;
                }
                enemyPosition += (KNOCKBACK_FORCE * knockbackDirection) * dt;
                enemySpriteDict.SetSprite("gel_turquoise");
            }
            else if (spawning)
            {
                animationTimer += dt;
                if (animationTimer >= 1)
                {
                    enemy.ChangeDirection();
                    spawning = false;
                }
            }
            else if (Dead)
            {
                animationTimer += dt;
                if (animationTimer >= 0.5)
                {
                    enemySpriteDict.Enabled = false;
                    enemy.Alive = false;
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
