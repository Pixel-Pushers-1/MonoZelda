using System.Collections.Generic;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;

namespace MonoZelda.Tiles.Doors;

internal static class DoorFactory
{
    public static IDoor CreateDoor(DoorSpawn door, ICommand roomTransitionCommand, CollisionController c)
    {
        var doorType = DoorTypeMap[door.Type];
        
        return doorType switch
        {
            DoorType.NormalDoor => new DungeonDoor(door, roomTransitionCommand, c),
            DoorType.LockedDoor => new KeyDoor(door, roomTransitionCommand, c),
            DoorType.BombableWall => new BombableWall(door, roomTransitionCommand, c),
            _ => throw new System.NotImplementedException(),
        };
    }
    
    private static readonly Dictionary<Dungeon1Sprite, DoorType> DoorTypeMap = new()
    {
        { Dungeon1Sprite.door_locked_east, DoorType.LockedDoor },
        { Dungeon1Sprite.door_locked_north, DoorType.LockedDoor },
        { Dungeon1Sprite.door_locked_south, DoorType.LockedDoor },
        { Dungeon1Sprite.door_locked_west, DoorType.LockedDoor },
        { Dungeon1Sprite.wall_bombed_east, DoorType.BombableWall },
        { Dungeon1Sprite.wall_bombed_north, DoorType.BombableWall },
        { Dungeon1Sprite.wall_bombed_south, DoorType.BombableWall },
        { Dungeon1Sprite.wall_bombed_west, DoorType.BombableWall },
        { Dungeon1Sprite.wall_east, DoorType.BombableWall },
        { Dungeon1Sprite.wall_north, DoorType.BombableWall },
        { Dungeon1Sprite.wall_south, DoorType.BombableWall },
        { Dungeon1Sprite.wall_west, DoorType.BombableWall },
        { Dungeon1Sprite.door_closed_east, DoorType.NormalDoor },
        { Dungeon1Sprite.door_closed_north, DoorType.NormalDoor },
        { Dungeon1Sprite.door_closed_south, DoorType.NormalDoor },
        { Dungeon1Sprite.door_closed_west, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_east, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_north, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_south, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_west, DoorType.NormalDoor },
    };
}