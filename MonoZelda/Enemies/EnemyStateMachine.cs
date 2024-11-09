using System;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Link;

namespace MonoZelda.Enemies
{
    public class EnemyStateMachine
    {
        //states
        public enum Direction { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None }
        private Direction direction = Direction.None;
        public bool Spawning {get; set; }
        public bool Dead { get; set; }
        private bool knockback;

        //not states
        private const float KNOCKBACK_FORCE = 1000f;
        private float velocity = 120;
        private float dt;
        private double animationTimer;
        private float knockbackTimer;
        private Vector2 knockbackDirection;
        private SpriteDict enemySpriteDict;
        private string currentSprite;
        private Vector2 movement;
        public Point currentPosition { get; private set; }

        public EnemyStateMachine(SpriteDict spriteDict)
        {
            enemySpriteDict = spriteDict;
            Spawning = true;
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

        public void Die(Boolean respawning)
        {
            if (respawning)
            {
                enemySpriteDict.Enabled = false;
            }
            else
            {
                Dead = true;
                animationTimer = 0;
                enemySpriteDict.SetSprite("death");
            }
        }

        public Point Update(Enemy enemy, Point position, GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 enemyPosition = position.ToVector2();
            if (knockback)
            {
                knockbackTimer += dt;
                if (knockbackTimer >= 0.05)
                {
                    velocity = 120;
                    knockback = false;
                    knockbackTimer = 0;
                }
                enemyPosition += (KNOCKBACK_FORCE * knockbackDirection) * dt;
                enemySpriteDict.SetSprite("gel_turquoise");
            }
            else if (Spawning)
            {
                animationTimer += dt;
                if (animationTimer >= 1)
                {
                    enemy.ChangeDirection();
                    Spawning = false;
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
                    Direction.UpLeft => new Vector2(-1, -1),
                    Direction.UpRight => new Vector2(1, -1),
                    Direction.DownLeft => new Vector2(-1, 1),
                    Direction.DownRight => new Vector2(1, 1),
                    _ => Vector2.Zero
                };
                enemyPosition += (velocity * movement)*dt;
                if (direction != Direction.None)
                {
                    enemySpriteDict.Enabled = true;
                }
                enemySpriteDict.SetSprite(currentSprite);
            }

            position = enemyPosition.ToPoint();
            enemySpriteDict.Position = position;
            currentPosition = position;
            return position;
        }
    }
}
