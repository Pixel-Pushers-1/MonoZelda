﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.UI;

internal class HUDMapWidget : ScreenWidget
{
    //singleton instance
    private static HUDMapWidget instance;

    //consts
    private static readonly Point roomSpacing = new(32, 16);
    private static readonly Point compassMarkerOffset = new(96 + 32 * 3, 80 - 16 * 4);

    //static data
    private static bool mapEnabled;
    private static Point playerMarkerOffset = new(96, 80);
    private static bool compassMarkerEnabled;

    //instance vars
    private static SpriteDict map;
    private static SpriteDict playerMarker;
    private static SpriteDict compassMarker;

    public HUDMapWidget(Screen screen, Point position, ContentManager cm) : base(screen, position)
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
    }

    public override void Draw(SpriteBatch sb)
    {
    }

    public override void Load(ContentManager content)
    {
    }

    public override void Update()
    {
        map.Position = WidgetLocation;
        playerMarker.Position = WidgetLocation + playerMarkerOffset;
        compassMarker.Position = WidgetLocation + compassMarkerOffset;
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

    public static void SetCompassMarkerVisible(bool visible) {
        compassMarkerEnabled = visible;
        compassMarker.Enabled = visible;
    }

    public static void SetMapVisible(bool visible) {
        mapEnabled = visible;
        map.Enabled = visible;
    }

    public static void Reset() {
        mapEnabled = false;
        playerMarkerOffset = new(96, 80);
        compassMarkerEnabled = false;
    }
}
