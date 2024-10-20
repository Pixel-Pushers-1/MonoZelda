using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies
{
    public class CardinalEnemyStateMachine
    {
        public enum Direction { Left, Right, Up, Down, None }

        private Direction direction = Direction.None;

        private int velocity = 1;

        public Point currentPosition { get; private set; }

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
            currentPosition = position;
            return position;
        }
    }
}
