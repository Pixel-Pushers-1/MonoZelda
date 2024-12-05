using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Loader;
using MonoZelda.Dungeons.Parser;
using MonoZelda.Save;

namespace MonoZelda.Scenes;

public class DungeonManager : IDungeonRoomLoader
{
    private RoomParser _roomParser;
    private Dictionary<string, DungeonRoom> _rooms { get; set; } = new();

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

    public void AddRandomRoom(string roomName, DungeonRoom room)
    {
        if (_rooms.ContainsKey(roomName))
        {
            _rooms[roomName] = room;
        }
        else 
        {
            _rooms.Add(roomName, room);
        }
    }

    public void Save(SaveState save)
    {
        save.Rooms = _rooms;
    }

    public void Load(SaveState save)
    {
        _rooms = save.Rooms;
    }
}