using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies.StalfosFolder
{
    public class StalfosStateMachine
    {
        public enum Direction { Left, Right, Up, Down, None }

        private Direction stalfosDirection;

        public void ChangeDirection(Direction newDirection)
        {
            stalfosDirection = newDirection;
        }

        public Point Update(Point position)
        {
            switch (stalfosDirection)
            {
                case Direction.Left:
                    position.X -= 1;
                    break;
                case Direction.Right:
                    position.X += 1;
                    break;
                case Direction.Up:
                    position.Y -= 1;
                    break;
                case Direction.Down:
                    position.Y += 1;
                    break;
            }
            return position;
        }
    }
}
