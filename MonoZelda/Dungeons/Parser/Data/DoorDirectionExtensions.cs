using System;

namespace MonoZelda.Dungeons.Parser.Data
{
    internal static class DoorDirectionExtensions
    {
        public static DoorDirection Reverse(this DoorDirection direction)
        {
            return direction switch
            {
                DoorDirection.North => DoorDirection.South,
                DoorDirection.South => DoorDirection.North,
                DoorDirection.East => DoorDirection.West,
                DoorDirection.West => DoorDirection.East,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
