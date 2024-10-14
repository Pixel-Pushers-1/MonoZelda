using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;
using MonoZelda.Sprites;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Sprites;

namespace PixelPushers.MonoZelda.Scenes;

public class ExperimentalDungeonLoader : IDungeonRoomLoader
{
    private static readonly HttpClient httpClient = new HttpClient();

    private static string Dungeon1 = "https://docs.google.com/spreadsheets/d/1LJPdekWHcv_nLglTE_mb_izUfJeQiEXoGHPhfGcHD-M/gviz/tq?tqx=out:csv&sheet=";

    private const int TileWidth = 64;
    private const int TileHeight = 64;
    private static readonly Point Margin = new Point(64, 64);
    private static readonly Point DungeonPosition = new Point(0, 192);

    private Texture2D _dungeonTexture;
    private GraphicsDevice _graphicsDevice;
    private CollisionController _collisionController;

    public ExperimentalDungeonLoader(ContentManager contentManager, CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        _dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);
        _graphicsDevice = graphicsDevice;
        _collisionController = collisionController;
    }

    public IDungeonRoom LoadRoom(string roomName)
    {

        using var dungeon1Stream = DownloadCsvStreamAsync(Dungeon1 + roomName).Result;

        if (dungeon1Stream != null)
        {
            using var streamReader = new StreamReader(dungeon1Stream);
            using TextFieldParser textFieldParser = new TextFieldParser(streamReader);
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(",");

            if (textFieldParser.EndOfData) return null;

            // First row contains the room definition
            string[] row = textFieldParser.ReadFields();

            IDungeonRoom room;
            LoadRoomPerimieter();

            if (row.Length > 1)
            {
                LoadRoomBackground(row[0]);
                var doors = LoadDoors(row);
                room = new ExampleDungeonRoom(doors);

                // The rest are collision shapes
                LoadCollisions(textFieldParser);

                return room;
            }
        }

        // TODO: Indicate error downloading/parsing CSV. An "ErrorRoom" would be cute. -js

        return null;
    }

    private void LoadRoomPerimieter()
    {
        var r = new SpriteDict(_dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Background, DungeonPosition);
        r.SetSprite(nameof(Dungeon1Sprite.room_exterior));
    }

    private void LoadRoomBackground(string backgroundSprite)
    {
        var roomPoint = new Point(128, 128);
        if (Enum.TryParse(backgroundSprite, out Dungeon1Sprite roomSprite))
        {
            var r = new SpriteDict(_dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Background, DungeonPosition + roomPoint);
            r.SetSprite(roomSprite.ToString());
        }
    }

    private List<IDoor> LoadDoors(string[] row)
    {
        var doorPositions = new Point[]
        {
            new (448, 0),
            new (896, 288),
            new (448, 576),
            new (0, 288)
        };

        var doors = new List<IDoor>();

        for (int i = 1; i <= 4; i++)
        {
            // Matching Door(sprite_name, destination)
            var doorRegex = new Regex(@"Door\((\w+_\w+)(?:,\s*(\w+))?\)");
            Match match = doorRegex.Match(row[i]);

            var spriteName = match.Groups[1].Value;
            var destination = match.Groups[2].Success ? match.Groups[2].Value : null;

            if (match.Success && Enum.TryParse(spriteName, out Dungeon1Sprite doorSprite))
            {
                // Origin is not the corner (0,0)
                var doorPosition = DungeonPosition + doorPositions[i - 1];

                var doorDict = new SpriteDict(_dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Blocks, doorPosition);
                doorDict.SetSprite(doorSprite.ToString());

                var door = new ExampleDoor(destination, doorPosition, TileWidth * 2, TileHeight * 2);
                doors.Add(door);
            }
        }

        return doors;
    }

    private void LoadCollisions(TextFieldParser textFieldParser)
    {
        var i = 0;

        while (!textFieldParser.EndOfData)
        {
            string[] fields = textFieldParser.ReadFields();
            for (int j = 0; j < fields.Length; j++)
            {
                if (Enum.TryParse(fields[j], out CollisionTileRect collisionRect))
                {
                    var position = new Point(j * TileWidth, i * TileHeight) + DungeonPosition + Margin;
                    var rect = GetCollisionRectangle(collisionRect, position, TileWidth, TileHeight);
                    var itemHitbox = new Collidable(rect, _graphicsDevice, "CollisionBlock");
                    _collisionController.AddCollidable(itemHitbox);
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

    private async Task<Stream> DownloadCsvStreamAsync(string url)
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