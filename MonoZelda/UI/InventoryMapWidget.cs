using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Dungeons;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.UI;

internal class InventoryMapWidget : ScreenWidget
{
    //singleton instance
    private static InventoryMapWidget instance;

    //consts
    private static readonly Point roomSpacing = new(32, 32);

    //static data
    private static Point playerMarkerOffset = (Point) DungeonConstants.GetRoomCoordinate("Room1") * roomSpacing + new Point(2, 11);
    private static HashSet<Point> discoveredRooms = new();

    //instance vars
    private SpriteDict playerMarker;
    private Dictionary<Point, SpriteDict> roomSpriteDicts;

    public InventoryMapWidget(Screen screen, Point position) : base(screen, position)
    {
        //set up singleton
        instance = this;
        //set up sprite dicts
        playerMarker = new(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + playerMarkerOffset);
        playerMarker.SetSprite("map_marker_white");
        roomSpriteDicts = new();
        foreach (Point coord in DungeonConstants.GetAllRoomCoordinates()) {
            Point roomPosition = WidgetLocation + coord * roomSpacing;
            roomSpriteDicts.Add(coord, new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, roomPosition));
            roomSpriteDicts[coord].SetSprite(DungeonConstants.GetRoomMapSprite(coord));
            roomSpriteDicts[coord].Enabled = discoveredRooms.Contains(coord);
            //enable starting room
            if (coord == (Point) DungeonConstants.GetRoomCoordinate("Room1")) {
                roomSpriteDicts[coord].Enabled = true;
            }
        }
    }

    public override void Draw(SpriteBatch sb)
    {
    }

    public override void Load(ContentManager content)
    {
    }

    public override void Update()
    {
        playerMarker.Position = WidgetLocation + playerMarkerOffset;
        foreach (var pair in roomSpriteDicts) {
            Point roomPosition = WidgetLocation + pair.Key * roomSpacing;
            pair.Value.Position = roomPosition;
        }
    }

    public void SetPlayerMapMarker(Point? coord)
    {
        if (coord == null) {
            playerMarker.Enabled = false;
        }
        else {
            playerMarkerOffset = roomSpacing * (Point) coord + new Point(2, 11);
            playerMarker.Position = WidgetLocation + playerMarkerOffset;
            if (discoveredRooms.Add((Point) coord)) {
                roomSpriteDicts[(Point) coord].Enabled = true;
            }
        }
    }

    public static void Reset()
    {
        playerMarkerOffset = (Point) DungeonConstants.GetRoomCoordinate("Room1") * roomSpacing + new Point(2, 11);
        discoveredRooms.Clear();
    }
}