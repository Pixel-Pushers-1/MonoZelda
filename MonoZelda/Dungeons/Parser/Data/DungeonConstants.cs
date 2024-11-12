
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.Dungeons;

public static class DungeonConstants
{
    public static readonly string DungeonOneUri = "https://docs.google.com/spreadsheets/d/1LJPdekWHcv_nLglTE_mb_izUfJeQiEXoGHPhfGcHD-M/gviz/tq?tqx=out:csv&sheet=";

    public static readonly int TileWidth = 64;
    public static readonly int TileHeight = 64;
    public static readonly Point Margin = new Point(64, 64);
    public static readonly Point DungeonPosition = new Point(0, 192);
    public static readonly Point BackgroundPosition = DungeonPosition + new Point(128, 128);

    public static Point RoomPosition => DungeonPosition + Margin;

    public static readonly Point[] DoorPositions = new Point[]
    {
        DungeonPosition + new Point(448, 0), // North Door
        DungeonPosition + new Point(896, 288), // East Door
        DungeonPosition + new Point(448, 576), // South Door
        DungeonPosition + new Point(0, 288) // West Door
    };

    public static readonly Dictionary<Direction, Point> adjacentTransitionRoomSpawnPoints = new()
    {
        { Direction.Left, new Point(1024,0) },
        { Direction.Right, new Point(-1024,0) },
        { Direction.Up, new Point(0,704) },
        { Direction.Down, new Point(0,-704) },
    };

    public static readonly Dictionary<Direction, Point> TransitionLinkSpawnPoints = new()
    {
        { Direction.Left, new Point(96,544) },
        { Direction.Right, new Point(928,544) },
        { Direction.Up, new Point(512,288) },
        { Direction.Down, new Point(512,800) },
    };

    public static readonly Dictionary<Direction, Vector2> DirectionVector = new()
    {
        { Direction.Left, new Vector2(-1,0) },
        { Direction.Right, new Vector2(1,0) },
        { Direction.Up, new Vector2(0,-1) },
        { Direction.Down, new Vector2(0,1) },
    };

    public static readonly Dictionary<Direction, Vector2> TransitionDirectionVector = new()
    {
        { Direction.Left, new Vector2(1,0) },
        { Direction.Right, new Vector2(-1,0) },
        { Direction.Up, new Vector2(0,1) },
        { Direction.Down, new Vector2(0,-1) },
    };

    public static Point? GetRoomCoordinate(string roomName)
    {
        return roomName switch {
            "Room1" => new Point(3,5),
            "Room2" => new Point(2,5),
            "Room3" => new Point(3,4),
            "Room4" => new Point(4,5),
            "Room5" => new Point(3,3),
            "Room6" => new Point(2,3),
            "Room7" => new Point(3,2),
            "Room8" => new Point(4,3),
            "Room9" => new Point(2,2),
            "Room10" => new Point(1,2),
            "Room11" => new Point(3,1),
            "Room12" => new Point(4,2),
            "Room13" => new Point(5,2),
            "Room14" => new Point(5,1),
            "Room15" => new Point(6,1),
            "Room16" => new Point(3,0),
            "Room17" => new Point(2,0),
            "Room18" => new Point(1,0),
            _ => null,
        };
    }
}
