﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace MonoZelda.Sprites;

public enum SpriteType {
    Player,
    Blocks,
    Items,
    Enemies,
    Title,
    Projectiles,
    Blank,
    HUD,
}

internal static class TextureData
{
    private const string PlayerFile = "Sprites/player";
    private const string BlocksFile = "Sprites/tiles_dungeon1";
    private const string ItemsFile = "Sprites/items";
    private const string EnemiesFile = "Sprites/enemies";
    private const string TitleFile = "Sprites/title";
    private const string HUDFile = "Sprites/hud";

    private static Texture2D PlayerTexture;
    private static Texture2D BlocksTexture;
    private static Texture2D ItemsTexture;
    private static Texture2D EnemiesTexture;
    private static Texture2D TitleTexture;
    private static Texture2D BlankTexture;
    private static Texture2D HUDTexture;

    public static void LoadTextures(ContentManager contentManager, GraphicsDevice graphicsDevice) {
        PlayerTexture = contentManager.Load<Texture2D>(PlayerFile);
        BlocksTexture = contentManager.Load<Texture2D>(BlocksFile);
        ItemsTexture = contentManager.Load<Texture2D>(ItemsFile);
        EnemiesTexture = contentManager.Load<Texture2D>(EnemiesFile);
        TitleTexture = contentManager.Load<Texture2D>(TitleFile);
        BlankTexture = new Texture2D(graphicsDevice, 1, 1);
        BlankTexture.SetData(new Color[] { Color.White });
        HUDTexture = contentManager.Load<Texture2D>(HUDFile);
    }

    public static Texture2D GetTexture(SpriteType type) {
        return type switch {
            SpriteType.Player => PlayerTexture,
            SpriteType.Blocks => BlocksTexture,
            SpriteType.Items => ItemsTexture,
            SpriteType.Enemies => EnemiesTexture,
            SpriteType.Title => TitleTexture,
            SpriteType.Projectiles => PlayerTexture,
            SpriteType.Blank => BlankTexture,
            SpriteType.HUD => HUDTexture,
            _ => BlankTexture,
        };
    }
}

internal static class SpriteCSVData
{
    private const string PlayerFile = "Content/Source Rect CSVs/Sprite Source Rects - Player.csv";
    private const string BlocksFile = "Content/Source Rect CSVs/Sprite Source Rects - Tiles Dungeon1.csv";
    private const string ItemsFile = "Content/Source Rect CSVs/Sprite Source Rects - Items.csv";
    private const string EnemiesFile = "Content/Source Rect CSVs/Sprite Source Rects - Enemies.csv";
    private const string TitleFile = "Content/Source Rect CSVs/Sprite Source Rects - Title.csv";
    private const string ProjectilesFile = "Content/Source Rect CSVs/Sprite Source Rects - Projectiles.csv";
    private const string HUDFile = "Content/Source Rect CSVs/Sprite Source Rects - HUD.csv";

    public static string GetFileName(SpriteType type) { 
        return type switch {
            SpriteType.Player => PlayerFile,
            SpriteType.Blocks => BlocksFile,
            SpriteType.Items => ItemsFile,
            SpriteType.Enemies => EnemiesFile,
            SpriteType.Title => TitleFile,
            SpriteType.Projectiles => ProjectilesFile,
            SpriteType.HUD => HUDFile,
            _ => "",
        };

    }
}

internal static class ColorData
{
    public static Color White { get; private set; } = new Color(1f, 1f, 1f, 1f);
    public static Color Red { get; private set; } = new Color(1f, .5f, .5f, 1f);
    public static Color Green { get; private set; } = new Color(.5f, 1f, .5f, 1f);
    public static Color Blue { get; private set; } = new Color(.5f, .5f, 1f, 1f);
    public static Color Transparent { get; private set; } = new Color(0f, 0f, 0f, 0f);
    public static Color Yellow { get; private set; } = new Color(1f, 1f, .5f, 1f);

}

