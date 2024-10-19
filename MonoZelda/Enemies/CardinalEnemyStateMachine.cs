using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies
{
    public class CardinalEnemyStateMachine
    {
        public enum Direction { Left, Right, Up, Down, None }

        private Direction direction;

        private int velocity = 1;

        public void ChangeDirection(Direction newDirection)
        {
            direction = newDirection;
        }

        public void ChangeSpeed(int newSpeed)
        {
            velocity = newSpeed;
        }

        public Point Update(Point position)
        {
            switch (direction)
            {
                case Direction.Left:
                    position.X -= velocity;
                    break;
                case Direction.Right:
                    position.X += velocity;
                    break;
                case Direction.Up:
                    position.Y -= velocity;
                    break;
                case Direction.Down:
                    position.Y += velocity;
                    break;
            }
            return position;
        }
    }
}
