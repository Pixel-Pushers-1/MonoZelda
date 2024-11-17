using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Sprites;

namespace MonoZelda.Dungeons
{
    public class DoorSpawn : RoomContent<Dungeon1Sprite>
    {
        public string Destination { get; }
        public Rectangle Bounds { get; set; }
        public DoorDirection Direction { get; set; }

        public DoorSpawn(string destination, DoorDirection direction, Point position, Dungeon1Sprite sprite, string room) 
            : base(position, sprite,room)
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
