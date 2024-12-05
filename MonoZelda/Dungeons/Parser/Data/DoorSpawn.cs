using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Sprites;
using System;

namespace MonoZelda.Dungeons
{
    [Serializable]
    public class DoorSpawn : RoomContent<Dungeon1Sprite>
    {
        public string Destination { get; set; }
        public Rectangle Bounds { get; set; }
        public DoorDirection Direction { get; set; }

        public DoorSpawn(string destination, DoorDirection direction, Point position, Dungeon1Sprite type, string roomName) 
            : base(position, type, roomName)
        {
            // Doors are 2x2 tiles
            var width = DungeonConstants.TileWidth * 2;
            var height = DungeonConstants.TileHeight * 2;

            Bounds = new Rectangle(position, new Point(width, height));

            Direction = direction;
            Destination = destination;
        }

    }
}
