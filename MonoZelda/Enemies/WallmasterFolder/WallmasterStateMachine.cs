﻿using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies.WallmasterFolder
{
    public class WallmasterStateMachine
    {
        public enum Direction { Left, Right, Up, Down }

        private Direction wallmasterDirection;

        public void ChangeDirection(Direction newDirection)
        {
            wallmasterDirection = newDirection;
        }

        public Point Update(Point position, GraphicsDeviceManager graphics)
        {
            switch (wallmasterDirection)
            {
                case Direction.Left:
                    if (position.X >= 0 + 32)
                    {
                        position.X -= 1;
                    }
                    break;
                case Direction.Right:
                    if (position.X <= graphics.PreferredBackBufferWidth - 32)
                    {
                        position.X += 1;
                    }
                    break;
                case Direction.Up:
                    if (position.Y >= 0 + 32)
                    {
                        position.Y -= 1;
                    }
                    break;
                case Direction.Down:
                    if (position.Y <= graphics.PreferredBackBufferHeight - 32)
                    {
                        position.Y += 1;
                    }
                    break;
            }
            return position;
        }
    }
}
