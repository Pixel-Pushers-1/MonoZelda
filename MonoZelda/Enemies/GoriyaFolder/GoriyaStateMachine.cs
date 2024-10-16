using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Enemies.GoriyaFolder
{
    public class GoriyaStateMachine
    {
        public enum Direction { Left, Right, Up, Down }

        private Direction goriyaDirection;

        public void ChangeDirection(Direction newDirection)
        {
            goriyaDirection = newDirection;
        }

        public Point Update(Point position, SpriteDict goriyaSpriteDict, GraphicsDeviceManager graphics)
        {
            switch (goriyaDirection)
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

        public void UpdateSprite(SpriteDict goriyaSpriteDict)
        {
            switch (goriyaDirection)
            {
                case Direction.Left:
                    goriyaSpriteDict.SetSprite("goriya_red_left");
                    break;
                case Direction.Right:
                    goriyaSpriteDict.SetSprite("goriya_red_right");
                    break;
                case Direction.Up:
                    goriyaSpriteDict.SetSprite("goriya_red_up");
                    break;
                case Direction.Down:
                    goriyaSpriteDict.SetSprite("goriya_red_down");
                    break;
            }
        }

    }
}
