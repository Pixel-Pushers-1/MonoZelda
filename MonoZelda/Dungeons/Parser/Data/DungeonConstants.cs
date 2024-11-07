
using Microsoft.Xna.Framework;

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
        DungeonPosition + new Point(448, 0),
        DungeonPosition + new Point(896, 288),
        DungeonPosition + new Point(448, 576),
        DungeonPosition + new Point(0, 288)
    };
}
