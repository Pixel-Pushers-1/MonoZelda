using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Loader;
using MonoZelda.Dungeons.Parser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class RoomParser
    {
        private IRoomLoader _roomLoader;
        private IRoomTokenizer _roomTokenizer;

        private readonly Dictionary<string, ICellParser> _cellParsers = new();

        public RoomParser(IRoomLoader loader, IRoomTokenizer tokenizer)
        {
            _roomLoader = loader;
            _roomTokenizer = tokenizer;


            _cellParsers.Add("item", new ItemCellParser());
            _cellParsers.Add("enemy", new EnemyCellParser());
            _cellParsers.Add("roomCollision", new RoomCollisionCellParser());
            _cellParsers.Add("boundaryCollision", new BoundaryCollisionCellParser());
            _cellParsers.Add("trigger", new TriggerCellParser());
        }

        public DungeonRoom Parse(string roomName)
        {
            var roomStream = _roomLoader.Load(roomName);
            var roomFile = _roomTokenizer.Tokenize(roomStream);

            if (roomFile == null) return null;

            var spriteType = ParseRoomBackground(roomFile.RoomSprite);
            var room = new DungeonRoom(roomName, spriteType);

            LoadDoors(roomFile, room);
            LoadContent(roomFile, room);
            

            return room;
        }

        private void LoadContent(RoomFile roomFile, DungeonRoom room)
        {
            for (int y = 0; y < roomFile.Content.Count; y++)
            {
                for (int x = 0; x < roomFile.Content[y].Count; x++)
                {
                    var position = new Point(x * DungeonConstants.TileWidth, y * DungeonConstants.TileHeight);
                    position += DungeonConstants.DungeonPosition + DungeonConstants.Margin;

                    var cell = roomFile.Content[y][x];

                    if (string.IsNullOrEmpty(cell)) continue;

                    InvokeParser(cell, position, room);
                }
            }
        }

        private static void LoadDoors(RoomFile file, DungeonRoom room)
        {
            var doorTokens = new List<string> { file.NorthDoor, file.EastDoor, file.SouthDoor, file.WestDoor };
            var doorDirections = new List<DoorDirection> { DoorDirection.North, DoorDirection.East, DoorDirection.South, DoorDirection.West };

            for (int i = 0; i < doorTokens.Count; i++)
            {
                var doorCellParser = new DoorCellParser(doorDirections[i]);
                var doorPosition = DungeonConstants.DoorPositions[i];
                doorCellParser.Parse(doorTokens[i], doorPosition, room);
            }
        }

        private void InvokeParser(string cell, Point position, DungeonRoom room)
        {
            // First part defines the Enum Type
            var enumType = cell[..cell.IndexOf('_')];
            // The rest is the enum value
            var enumValue = cell[(cell.IndexOf('_') + 1)..];

            if (!_cellParsers.TryGetValue(enumType, out var cellParser)) return;

            cellParser.Parse(enumValue, position, room);
        }

        private static Dungeon1Sprite ParseRoomBackground(string backgroundSprite)
        {
            if (Enum.TryParse(backgroundSprite, out Dungeon1Sprite roomSprite))
            {
                return roomSprite;
            }

            return Dungeon1Sprite.unknown;
        }
    }
}
