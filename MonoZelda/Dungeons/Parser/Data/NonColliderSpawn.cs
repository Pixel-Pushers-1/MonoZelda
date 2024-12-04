using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Dungeons.Parser.Data
{
    [Serializable]
    public class NonColliderSpawn
    {
        public Point Position { get; set; }

        public NonColliderSpawn(Point position)
        {
            Position = position;
        }

    }
}
