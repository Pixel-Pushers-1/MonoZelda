using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Loader;
using MonoZelda.Dungeons.Parser;

namespace MonoZelda.Scenes;

public class DungeonManager : IDungeonRoomLoader
{
    private RoomParser _roomParser;
    private readonly Dictionary<string, DungeonRoom> _rooms = new();

    public DungeonManager()
    {
        var loader = new HTTPRoomStream(DungeonConstants.DungeonOneUri);
        var tokenizer = new RoomTokenizer();
        _roomParser = new RoomParser(loader, tokenizer);
    }

    public IDungeonRoom LoadRoom(string roomName)
    {
        if (_rooms.ContainsKey(roomName))
        {
            return _rooms[roomName];
        }

        var room = _roomParser.Parse(roomName);

        _rooms.Add(roomName, room);
        return room;
    }
}