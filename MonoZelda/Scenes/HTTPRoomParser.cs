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
using MonoZelda.Collision;
using MonoZelda.Dungeons;
using MonoZelda.Enemies;
using MonoZelda.Items;
using MonoZelda.Sprites;
using MonoZelda.Trigger;

namespace MonoZelda.Scenes;

public class HTTPRoomParser : IDungeonRoomLoader
{
    private static readonly HttpClient httpClient = new HttpClient();

    private Texture2D _dungeonTexture;

    private GraphicsDevice _graphicsDevice;

    public HTTPRoomParser(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        _dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);
        _graphicsDevice = graphicsDevice;

    }

    public IDungeonRoom LoadRoom(string roomName)
    {

        using var dungeon1Stream = DownloadCsvStreamAsync(DungeonConstants.Dungeon1 + roomName).Result;

        if (dungeon1Stream != null)
        {
            using var streamReader = new StreamReader(dungeon1Stream);
            using TextFieldParser textFieldParser = new TextFieldParser(streamReader);
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(",");

            if (textFieldParser.EndOfData) return null;

            // First row contains the room definition
            string[] row = textFieldParser.ReadFields();

            //IDungeonRoom room;

            if (row.Length > 1)
            {
                // First row is room sprite and doors.
                var roomSprite = ParseRoomBackground(row[0]);
                var doors = LoadDoors(row);

                var dungeonRoom = new DungeonRoom(roomName, roomSprite, doors);
                LoadContent(textFieldParser, dungeonRoom);
                return dungeonRoom;
            }
        }

        // TODO: Indicate error downloading/parsing CSV. An "ErrorRoom" would be cute. -js

        return null;
    }

    private Dungeon1Sprite ParseRoomBackground(string backgroundSprite)
    {
        if (Enum.TryParse(backgroundSprite, out Dungeon1Sprite roomSprite))
        {
            return roomSprite;
        }

        return Dungeon1Sprite.unknown;
    }

    private List<IDoor> LoadDoors(string[] row)
    {
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
                var doorPosition = DungeonConstants.DoorPositions[i - 1];

                // Doors are 2x2 tiles
                var width = DungeonConstants.TileWidth * 2;
                var height = DungeonConstants.TileHeight * 2;
                var door = new ExampleDoor(destination, doorPosition, width, height, doorSprite);
                doors.Add(door);
            }
        }

        return doors;
    }

    private void LoadContent(TextFieldParser parser, DungeonRoom room)
    {
        var i = 0;

        while (!parser.EndOfData)
        {
            string[] fields = parser.ReadFields();

            // Itterating the cells
            for (int j = 0; j < fields.Length; j++)
            {
                var position = new Point(j * DungeonConstants.TileWidth, i * DungeonConstants.TileHeight) + DungeonConstants.DungeonPosition + DungeonConstants.Margin;
                var value = fields[j];

                if (string.IsNullOrEmpty(value)) continue;

                // First part defines the Enum Type
                var enumType = value.Substring(0, value.IndexOf('_'));
                // The rest is the enum value
                var enumValue = value.Substring(value.IndexOf('_') + 1);
                switch (enumType)
                {
                    case "item":
                        // Load item
                        if (Enum.TryParse(enumValue, out ItemList item))
                        {
                            var itemSpawn = new ItemSpawn(position, item);
                            room.AddItemSpawn(itemSpawn);
                        }
                        break;
                    case "enemy":
                        // Load enemy
                        if (Enum.TryParse(enumValue, out EnemyList enemy))
                        {
                            var enemySpawn = new EnemySpawn(position, enemy);
                            room.AddEnemySpawn(enemySpawn);
                        }
                        break;
                    case "roomCollision":
                        // Load collision
                        if (Enum.TryParse(enumValue, out CollisionTileRect collisionRect))
                        {
                            var rect = GetCollisionRectangle(collisionRect, position, DungeonConstants.TileWidth, DungeonConstants.TileHeight);
                            room.AddStaticRoomCollider(rect);
                        }
                        break;
                    case "boundaryCollision":
                        // Load boundary collision
                        if (Enum.TryParse(enumValue, out CollisionTileRect boundaryRect))
                        {
                            var rect = GetCollisionRectangle(boundaryRect, position, DungeonConstants.TileWidth, DungeonConstants.TileHeight);
                            room.AddStaticBoundaryCollider(rect);
                        }
                        break;
                    case "trigger":
                        // Load trigger
                        if (Enum.TryParse(enumValue, out TriggerType trigger))
                        {
                            var triggerSpawn = new TriggerSpawn(position, trigger);
                            room.AddTrigger(triggerSpawn);
                        }
                        break;
                    default:
                        break;
                }
            }

            // next row
            i++;
        }
    }

    private Rectangle GetCollisionRectangle(CollisionTileRect collisionRect, Point position, int tileWidth, int tileHeight)
    {
        return collisionRect switch
        {
            CollisionTileRect.top => new Rectangle(position, new Point(tileWidth, tileHeight / 2)),
            CollisionTileRect.bottom => new Rectangle(new Point(position.X, position.Y + tileHeight / 2), new Point(tileWidth, tileHeight / 2)),
            CollisionTileRect.left => new Rectangle(position, new Point(tileWidth / 2, tileHeight)),
            CollisionTileRect.right => new Rectangle(new Point(position.X + tileWidth / 2, position.Y), new Point(tileWidth / 2, tileHeight)),
            CollisionTileRect.full or _ => new Rectangle(position, new Point(tileWidth, tileHeight)),
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