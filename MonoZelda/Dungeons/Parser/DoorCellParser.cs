using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using System;
using System.Text.RegularExpressions;

namespace MonoZelda.Dungeons.Parser
{
    internal class DoorCellParser : ICellParser
    {
        private DoorDirection direction { get; }
        public DoorCellParser(DoorDirection direction)
        {
            this.direction = direction;
        }

        public void Parse(string cell, Point position, DungeonRoom room)
        {
            // Matching Door(sprite_name, destination)
            var doorRegex = new Regex(@"Door\((\w+_\w+)(?:,\s*(\w+))?\)");
            Match match = doorRegex.Match(cell);

            var spriteName = match.Groups[1].Value;
            var destination = match.Groups[2].Success ? match.Groups[2].Value : null;

            if (match.Success && Enum.TryParse(spriteName, out Dungeon1Sprite doorSprite))
            {
                // Doors are 2x2 tiles
                var width = DungeonConstants.TileWidth * 2;
                var height = DungeonConstants.TileHeight * 2;
                var door = new DoorSpawn(destination, direction, position, doorSprite, room.RoomName);

                room.AddDoor(door);
            }
        }
    }
}
