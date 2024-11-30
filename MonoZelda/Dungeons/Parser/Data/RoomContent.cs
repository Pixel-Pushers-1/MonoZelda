using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Dungeons
{
    [Serializable]
    public abstract class RoomContent<T> where T : Enum
    {
        public string RoomName { get; set; }
        public Point Position { get; private set; }
        public T Type { get; set; }

        public RoomContent(Point position, T type, string roomName) 
        {
            RoomName = roomName;
            Position = position;
            Type = type;
        }
    }
}
