using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Loader;
using MonoZelda.Dungeons.Parser;

namespace MonoZelda.Scenes;

public class DungeonManager : IDungeonRoomLoader
{
    private IRoomLoader _roomStream;
    private IRoomTokenizer _roomTokenizer;

    private readonly Dictionary<string, DungeonRoom> _rooms = new();
    private readonly Dictionary<string, ICellParser> _cellParsers = new();

    public DungeonManager(IRoomLoader loader, IRoomTokenizer tokenizer)
    {
        _roomStream = loader;
        _roomTokenizer = tokenizer;

        _cellParsers.Add("item", new ItemCellParser());
        _cellParsers.Add("enemy", new EnemyCellParser());
        _cellParsers.Add("collision", new CollisionCellParser());
        _cellParsers.Add("trigger", new TriggerCellParser());
    }

    public IDungeonRoom LoadRoom(string roomName)
    {
        if (_rooms.ContainsKey(roomName))
        {
            return _rooms[roomName];
        }

        var roomStream = _roomStream.Load(roomName);
        var roomFile = _roomTokenizer.Tokenize(roomStream);

        var spriteType = ParseRoomBackground(roomFile.RoomSprite);

        var dungeonRoom = new DungeonRoom(roomName, spriteType);
        LoadDoors(roomFile, dungeonRoom);
        LoadContent(roomFile, dungeonRoom);

        _rooms.Add(roomName, dungeonRoom);
        return dungeonRoom;
    }

    private Dungeon1Sprite ParseRoomBackground(string backgroundSprite)
    {
        if (Enum.TryParse(backgroundSprite, out Dungeon1Sprite roomSprite))
        {
            return roomSprite;
        }

        return Dungeon1Sprite.unknown;
    }

    private static void LoadDoors(RoomFile file, DungeonRoom room)
    {
        var doorTokens = new List<string> { file.NorthDoor, file.EastDoor, file.SouthDoor, file.WestDoor };
        var doorCellParser = new DoorCellParser();

        for (int i = 0; i < doorTokens.Count; i++)
        {
            var doorPosition = DungeonConstants.DoorPositions[i];
            doorCellParser.Parse(doorTokens[i], doorPosition, room);
        }
    }

    private void LoadContent(RoomFile file, DungeonRoom room)
    {
        for(int i = 0; i< file.Content.Count; i ++)
        {
            var fields = file.Content[i];

            // Itterating the cells
            for (int j = 0; j < fields.Count; j++)
            {
                var position = new Point(j * DungeonConstants.TileWidth, i * DungeonConstants.TileHeight) + DungeonConstants.DungeonPosition + DungeonConstants.Margin;
                var value = fields[j];

                if (string.IsNullOrEmpty(value)) continue;

                // First part defines the Enum Type
                var enumType = value.Substring(0, value.IndexOf('_'));
                // The rest is the enum value
                var enumValue = value.Substring(value.IndexOf('_') + 1);

                if (!_cellParsers.ContainsKey(enumType)) continue;

                var cellParser = _cellParsers[enumType];
                cellParser.Parse(enumValue, position, room);
            }
        }
    }

}