using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.UI;

internal class HUDMapWidget : ScreenWidget
{
    //singleton instance
    private static HUDMapWidget instance;

    //consts
    private static readonly Point roomSpacing = new(32, 16);
    private static readonly Point compassMarkerOffset = new(192, 16);
    private static readonly Point mapItemSpriteOffset = new(124,-348);
    private static readonly Point compassItemSpriteOffset = new(108,-196);

    //static data
    private static bool mapEnabled;
    private static Point playerMarkerOffset = (Point) DungeonConstants.GetRoomCoordinate("Room1") * roomSpacing;
    private static bool compassMarkerEnabled;

    //instance vars
    private SpriteDict map;
    private SpriteDict playerMarker;
    private SpriteDict compassMarker;
    private SpriteDict compassItem;
    private SpriteDict mapItem;

    public HUDMapWidget(Screen screen, Point position) : base(screen, position)
    {
        //set up singleton
        instance = this;

        //set up sprite dicts
        map = new(SpriteType.HUD, SpriteLayer.HUD, position);
        map.SetSprite("hud_map");
        map.Enabled = mapEnabled;
        playerMarker = new(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + playerMarkerOffset);
        playerMarker.SetSprite("map_marker_white");
        compassMarker = new(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + compassMarkerOffset);
        compassMarker.SetSprite("map_marker_red_blinking");
        compassMarker.Enabled = compassMarkerEnabled;
        compassItem = new(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + mapItemSpriteOffset);
        compassItem.SetSprite("compass");
        compassItem.Enabled = compassMarkerEnabled;
        mapItem = new(SpriteType.HUD, SpriteLayer.HUD + 1, WidgetLocation + compassItemSpriteOffset);
        mapItem.SetSprite("map");
        mapItem.Enabled = mapEnabled;
    }

    public override void Draw(SpriteBatch sb)
    {
    }

    public override void Load(ContentManager content)
    {
    }

    public override void Update()
    {
        SetCompassMarkerVisible(PlayerState.HasCompass);
        SetMapVisible(PlayerState.HasMap);

        map.Position = WidgetLocation;
        playerMarker.Position = WidgetLocation + playerMarkerOffset;
        compassMarker.Position = WidgetLocation + compassMarkerOffset;
        compassItem.Position = WidgetLocation + compassItemSpriteOffset;
        mapItem.Position = WidgetLocation + mapItemSpriteOffset;
    }

    public void SetPlayerMapMarker(Point? coord) {
        if (coord == null) {
            playerMarker.Enabled = false;
        }
        else {
            playerMarkerOffset = roomSpacing * (Point)coord;
            playerMarker.Position = WidgetLocation + playerMarkerOffset;
        }
    }

    private void SetCompassMarkerVisible(bool visible) {
        compassMarkerEnabled = visible;
        instance.compassMarker.Enabled = visible;
        instance.compassItem.Enabled = visible; 
    }

    private void SetMapVisible(bool visible) {
        mapEnabled = visible;
        instance.map.Enabled = visible;
        instance.mapItem.Enabled = visible;
    }

    public static void Reset() {
        mapEnabled = false;
        playerMarkerOffset = (Point) DungeonConstants.GetRoomCoordinate("Room1") * roomSpacing;
        compassMarkerEnabled = false;
    }
}

