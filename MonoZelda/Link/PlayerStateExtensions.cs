using System;

namespace MonoZelda.Link;

public static class PlayerStateExtensions
{
    public static Direction Reverse(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException("Invalid direction"),
        };
    }
}
