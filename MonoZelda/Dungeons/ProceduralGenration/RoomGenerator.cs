using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Scenes;
using System;
using Microsoft.Xna.Framework;

namespace MonoZelda.Dungeons;

public class RoomGenerator
{
    private const int POOL_SIZE = 11;
    private const string ROOM_NAME = "RandomRoom";
    private static readonly string[] RoomPool = {"Room1","Room2","Room3","Room4","Room5","Room8",
                               "Room11","Room12","Room13","Room14","Room16"};
    private static readonly Point eastDoorStaticCollider = new Point(960, 576);

    private int randomRoomNum;
    private Random rnd;
    private IDungeonRoomLoader roomManager;

    public RoomGenerator(IDungeonRoomLoader roomManager)
    {
        this.roomManager = roomManager;
        rnd = new Random();
    }

    public IDungeonRoom GetRandomRoom(IDungeonRoomLoader roomManager)
    {
        // Get loaded data for a random room
        randomRoomNum = rnd.Next(POOL_SIZE);
        string dungeonRoomString = RoomPool[randomRoomNum];

        // get preloaded room for roomSprite and collider data
        IDungeonRoom dungeonRoomData = roomManager.LoadRoom(dungeonRoomString);

        // create RandomRoom
        var randomRoom = new DungeonRoom(ROOM_NAME, dungeonRoomData.RoomSprite);
        roomManager.AddRandomRoom(ROOM_NAME, randomRoom);

        // create doors for the room
        AddDoors(randomRoom);

        // add static colliders
        AddStaticColliders(randomRoom, dungeonRoomData);

        // add non collider spawns
        AddNonColliderSpawns(randomRoom, dungeonRoomData);

        return randomRoom;

    }

    private void AddDoors(DungeonRoom randomRoom)
    {
        // north door is a wall
        Point NorthDoorPosition = DungeonConstants.DoorPositions[0];
        var northDoorSpawn = new DoorSpawn(null, DoorDirection.North, NorthDoorPosition, Dungeon1Sprite.wall_north, randomRoom.RoomSprite.ToString());
        randomRoom.AddDoor(northDoorSpawn);

        // east door is diamond door
        Point EastDoorPosition = DungeonConstants.DoorPositions[1];
        var eastDoorSpawn = new DoorSpawn(ROOM_NAME, DoorDirection.East, EastDoorPosition, Dungeon1Sprite.door_closed_east, randomRoom.RoomSprite.ToString());
        randomRoom.AddDoor(eastDoorSpawn);

        // south door is a wall
        Point SouthDoorPosition = DungeonConstants.DoorPositions[2];
        var southDoorSpawn = new DoorSpawn(null, DoorDirection.South, SouthDoorPosition, Dungeon1Sprite.wall_south, randomRoom.RoomSprite.ToString());
        randomRoom.AddDoor(southDoorSpawn);

        // west door is a wall
        Point WestDoorPosition = DungeonConstants.DoorPositions[3];
        var westDoorSpawn = new DoorSpawn(null, DoorDirection.West, WestDoorPosition, Dungeon1Sprite.wall_west, randomRoom.RoomSprite.ToString());
        randomRoom.AddDoor(westDoorSpawn);
    }

    private void AddStaticColliders(DungeonRoom randomRoom, IDungeonRoom dungeonRoomData)
    {
        // Add static boundary colliders
        foreach (var staticCollider in dungeonRoomData.GetStaticBoundaryColliders())
        {
            randomRoom.AddStaticBoundaryCollider(staticCollider);
        }

        // Add static room colliders
        foreach (var roomCollider in dungeonRoomData.GetStaticRoomColliders())
        {
            randomRoom.AddStaticRoomCollider(roomCollider);
        }
    }

    private void AddNonColliderSpawns(DungeonRoom randomRoom, IDungeonRoom dungeonRoomData)
    {
        // Add non collider spawns
        foreach (var nonColliderSpawn in dungeonRoomData.GetNonColliderSpawns())
        {
            randomRoom.AddNonColliderSpawn(nonColliderSpawn);
        }
    }
}