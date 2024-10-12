using System;
using System.IO;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Dungeons;
using MonoZelda.Sprites;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Scenes;

public class ExperimentalDungeonLoader
{
    private static readonly HttpClient httpClient = new HttpClient();

    private static string Dungeon1 = "https://docs.google.com/spreadsheets/d/1LJPdekWHcv_nLglTE_mb_izUfJeQiEXoGHPhfGcHD-M/gviz/tq?tqx=out:csv&sheet=Room1";

    private const int TileWidth = 64;
    private const int TileHeight = 64;
    private static readonly Point Margin = new Point(64, 64);
    private static readonly Point DungeonPoint = new Point(0, 192);

    public ExperimentalDungeonLoader(ContentManager contentManager, CollidablesManager cm, GraphicsDevice graphicsDevice)
    {
        var texture = contentManager.Load<Texture2D>(TextureData.Blocks);
        LoadBorder(texture);
        LoadDungeon(contentManager, cm, graphicsDevice, texture);
    }

    private void LoadBorder(Texture2D texture)
    {
        var roomPoint = new Point(128, 128);
        var d = new SpriteDict(texture, SpriteCSVData.Blocks, SpriteLayer.Background, DungeonPoint);
        d.SetSprite("room_exterior");
    }

    private void LoadDungeon(ContentManager contentManager, CollidablesManager cm, GraphicsDevice graphicsDevice, Texture2D texture)
    {
        using var dungeon1Stream = DownloadCsvStreamAsync(Dungeon1).Result;

        if (dungeon1Stream != null)
        {
            using var streamReader = new StreamReader(dungeon1Stream);
            using TextFieldParser textFieldParser = new TextFieldParser(streamReader);
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(",");

            if (textFieldParser.EndOfData) return;

            string[] roomDef = textFieldParser.ReadFields();
            LoadRoomBackground(contentManager, DungeonPoint, roomDef);
            LoadDoors(texture, DungeonPoint, roomDef);
            LoadCollisions(textFieldParser, cm, graphicsDevice, DungeonPoint);
        }

        // TODO: Indicate error downloading CSV
    }

    private void LoadRoomBackground(ContentManager contentManager, Point dungeonPoint, string[] roomDef)
    {
        var roomPoint = new Point(128, 128);
        if (Enum.TryParse(roomDef[0], out Dungeon1Sprite roomSprite))
        {
            var r = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, SpriteLayer.Background, dungeonPoint + roomPoint);
            r.SetSprite(roomSprite.ToString());
        }
    }

    private void LoadDoors(Texture2D texture, Point dungeonPoint, string[] roomDef)
    {
        var doorPoints = new Point[]
        {
            new (448, 0),
            new (896, 288),
            new (448, 576),
            new (0, 288)
        };

        for (int i = 1; i <= 4; i++)
        {
            if (Enum.TryParse(roomDef[i], out Dungeon1Sprite doorSprite))
            {
                var doorDict = new SpriteDict(texture, SpriteCSVData.Blocks, SpriteLayer.Blocks, dungeonPoint + doorPoints[i - 1]);
                doorDict.SetSprite(doorSprite.ToString());
            }
        }
    }

    private void LoadCollisions(TextFieldParser textFieldParser, CollidablesManager cm, GraphicsDevice graphicsDevice, Point dungeonPoint)
    {
        var i = 0;

        while (!textFieldParser.EndOfData)
        {
            string[] fields = textFieldParser.ReadFields();
            for (int j = 0; j < fields.Length; j++)
            {
                if (Enum.TryParse(fields[j], out CollisionTileRect collisionRect))
                {
                    var position = new Point(j * TileWidth, i * TileHeight) + dungeonPoint + Margin;
                    var rect = GetCollisionRectangle(collisionRect, position, TileWidth, TileHeight);
                    var itemHitbox = new Collidable(rect, graphicsDevice);
                    cm.AddHitbox(itemHitbox);
                }
            }
            i++;
        }
    }

    private Rectangle GetCollisionRectangle(CollisionTileRect collisionRect, Point position, int tileWidth, int tileHeight)
    {
        return collisionRect switch
        {
            CollisionTileRect.collision_top => new Rectangle(position, new Point(tileWidth, tileHeight / 2)),
            CollisionTileRect.collision_bottom => new Rectangle(new Point(position.X, position.Y + tileHeight / 2), new Point(tileWidth, tileHeight / 2)),
            CollisionTileRect.collision_left => new Rectangle(position, new Point(tileWidth / 2, tileHeight)),
            CollisionTileRect.collision_right => new Rectangle(new Point(position.X + tileWidth / 2, position.Y), new Point(tileWidth / 2, tileHeight)),
            CollisionTileRect.collision_full or _ => new Rectangle(position, new Point(tileWidth, tileHeight)),
        };
    }

    public async Task<Stream> DownloadCsvStreamAsync(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading CSV: {ex.Message}");
            return null;
        }
    }
}