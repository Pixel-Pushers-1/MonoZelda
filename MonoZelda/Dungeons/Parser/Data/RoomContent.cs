using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Dungeons
{
    public abstract class RoomContent<T> where T : Enum
    {
        public Point Position { get; private set; }
        public T Type { get; set; }

        public RoomContent(Point position, T type) 
        {
            Position = position;
            Type = type;
        }
    }
}
