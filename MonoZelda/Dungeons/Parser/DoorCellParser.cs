using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class DoorCellParser : ICellParser
    {
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
                var door = new ExampleDoor(destination, position, width, height, doorSprite);

                room.AddDoor(door);
            }
        }
    }
}
