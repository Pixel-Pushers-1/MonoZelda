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
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Scenes;

public class ExperimentalDungeonLoader
{
    private static readonly HttpClient httpClient = new HttpClient();

    private static string Dungeon1 = "https://docs.google.com/spreadsheets/d/1LJPdekWHcv_nLglTE_mb_izUfJeQiEXoGHPhfGcHD-M/gviz/tq?tqx=out:csv&sheet=Room1";

    public ExperimentalDungeonLoader(ContentManager contentManager, CollidablesManager cm, GraphicsDevice graphicsDevice)
    {
        // Load the perimeter
        var dungonPoint = new Point(0, 192);
        var roomPoint = new Point(128, 128);
        var northDoorPoint = new Point(448, 0);
        var eastDoorPoint = new Point(896, 288);
        var southDoorPoint = new Point(448, 576);
        var westDoorPoint = new Point(0, 288);

        var d = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, -10, dungonPoint);
        d.SetSprite("room_exterior");

        // Load the dungeon from the URI
        using var dungeon1Stream = DownloadCsvStream(Dungeon1);

        if (dungeon1Stream != null)
        {
            using var streamReader = new StreamReader(dungeon1Stream);

            // Set up text parser
            using TextFieldParser textFieldParser = new TextFieldParser(streamReader);
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(",");

            // Nothing to do if the file is empty
            if (textFieldParser.EndOfData)
            {
                return;
            }

            // First row describes the room bg
            string[] roomDef = textFieldParser.ReadFields();
            // roomDef[0] should be a Dungeon1 sprite
            if (Enum.TryParse(roomDef[0], out Dungeon1Sprite roomSprite))
            {
                var r = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 0, dungonPoint + roomPoint);
                r.SetSprite(roomSprite.ToString());
            }

            // Cells 1-4 describe the doors
            SpriteDict doorDict = null;
            if (Enum.TryParse(roomDef[1], out Dungeon1Sprite northDoorSprite))
            {

                doorDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 1, dungonPoint + northDoorPoint);
                doorDict.SetSprite(northDoorSprite.ToString());
            }
            if (Enum.TryParse(roomDef[2], out Dungeon1Sprite eastDoorSprite))
            {
                doorDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 1, dungonPoint + eastDoorPoint);
                doorDict.SetSprite(eastDoorSprite.ToString());
            }
            if (Enum.TryParse(roomDef[3], out Dungeon1Sprite southDoorSprite))
            {
                doorDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 1, dungonPoint + southDoorPoint);
                doorDict.SetSprite(southDoorSprite.ToString());
            }
            if (Enum.TryParse(roomDef[4], out Dungeon1Sprite westDoorSprite))
            {
                doorDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 1, dungonPoint + westDoorPoint);
                doorDict.SetSprite(westDoorSprite.ToString());
            }

            // Loop through CSV file
            var i = 0;
            var tileWidth = 64;
            var tileHeight = 64;
            var margin = new Point(64,64);
            while (!textFieldParser.EndOfData)
            {
                string[] fields = textFieldParser.ReadFields();
                var j = 0;

                foreach (var field in fields)
                {
                    // Try and parse the field as a BlockType enum
                    if (Enum.TryParse(field, out CollisionTileRect collisionRect))
                    {
                        var position = new Point(j * tileWidth, i * tileHeight) + dungonPoint + margin;

                        var rectSize = new Point(tileWidth, tileHeight);
                        Rectangle rect;

                        switch (collisionRect)
                        {
                            case CollisionTileRect.collision_top:
                                rectSize = new Point(tileWidth, tileHeight / 2);
                                rect = new Rectangle(position, rectSize);
                                break;

                            case CollisionTileRect.collision_bottom:
                                rectSize = new Point(tileWidth, tileHeight / 2);
                                rect = new Rectangle(new Point(position.X, position.Y + tileHeight / 2), rectSize);
                                break;

                            case CollisionTileRect.collision_left:
                                rectSize = new Point(tileWidth / 2, tileHeight);
                                rect = new Rectangle(position, rectSize);
                                break;

                            case CollisionTileRect.collision_right:
                                rectSize = new Point(tileWidth / 2, tileHeight);
                                rect = new Rectangle(new Point(position.X + tileWidth / 2, position.Y), rectSize);
                                break;

                            case CollisionTileRect.collision_full:
                            default:
                                rect = new Rectangle(position, new Point(tileWidth, tileHeight));
                                break;
                        }

                        Collidable itemHitbox = new Collidable(rect, graphicsDevice);
                        cm.AddHitbox(itemHitbox);
                    }

                    j++;
                }

                i++;
            }
        }
    }

    public Stream DownloadCsvStream(string url)
    {
        try
        {
            // Send a synchronous GET request to the specified URL
            var response = httpClient.GetAsync(url).Result;
            
            // Ensure that the request was successful
            response.EnsureSuccessStatusCode();

            // Return the response content as a stream (CSV content)
            return response.Content.ReadAsStreamAsync().Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading CSV: {ex.Message}");
            return null;
        }
    }
}