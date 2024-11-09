using System.Collections.Generic;
using System.ComponentModel;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Enemies;
using MonoZelda.Scenes;

namespace MonoZelda.Tiles.Doors;

internal static class DoorFactory
{
    public static IDoor CreateDoor(DoorSpawn door, ICommand roomTransitionCommand, CollisionController c, List<IEnemy> enemies)
    {
        var doorType = DoorTypeMap[door.Type];
        
        return doorType switch
        {
            DoorType.NormalDoor => new DungeonDoor(door, roomTransitionCommand, c),
            DoorType.LockedDoor => new KeyDoor(door, roomTransitionCommand, c),
            DoorType.BombableWall => new BombableWall(door, roomTransitionCommand, c),
            DoorType.DiamondDoor => new DiamondDoor(door, roomTransitionCommand, c, enemies),
            DoorType.Wall => new Wall(door, c),
            _ => throw new InvalidEnumArgumentException()
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
        { Dungeon1Sprite.wall_east, DoorType.Wall },
        { Dungeon1Sprite.wall_north, DoorType.Wall },
        { Dungeon1Sprite.wall_south, DoorType.Wall },
        { Dungeon1Sprite.wall_west, DoorType.Wall },
        { Dungeon1Sprite.bombable_wall_east, DoorType.BombableWall },
        { Dungeon1Sprite.bombable_wall_north, DoorType.BombableWall },
        { Dungeon1Sprite.bombable_wall_south, DoorType.BombableWall },
        { Dungeon1Sprite.bombable_wall_west, DoorType.BombableWall },
        { Dungeon1Sprite.diamond_door_east, DoorType.DiamondDoor },
        { Dungeon1Sprite.diamond_door_north, DoorType.DiamondDoor },
        { Dungeon1Sprite.diamond_door_south, DoorType.DiamondDoor },
        { Dungeon1Sprite.diamond_door_west, DoorType.DiamondDoor },
        { Dungeon1Sprite.door_open_east, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_north, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_south, DoorType.NormalDoor },
        { Dungeon1Sprite.door_open_west, DoorType.NormalDoor },
    };
}