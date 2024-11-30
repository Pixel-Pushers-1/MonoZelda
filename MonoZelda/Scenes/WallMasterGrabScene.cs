﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Dungeons;
using System.Collections.Generic;
using MonoZelda.Commands;

namespace MonoZelda.Scenes;

public class WallMasterGrabScene : Scene
{
    // movement zones; Integer represent priority of zones in case player is in two movement zones
    private static readonly (Rectangle, int) MoveUpZone = (new Rectangle(new Point(832, 384), new Point(64, 160)), 1);
    private static readonly (Rectangle, int) MoveDownZone = (new Rectangle(new Point(832, 544), new Point(64, 160)), 1);
    private static readonly (Rectangle,int) MoveLeftZoneTop = (new Rectangle(new Point(192, 320), new Point(704, 64)),2);
    private static readonly (Rectangle,int) MoveLeftZoneBottom = (new Rectangle(new Point(192, 704), new Point(704, 64)), 2);
    private static readonly (Rectangle, int) MoveDownDoorZone = (new Rectangle(new Point(128, 320), new Point(64, 192)), 3);
    private static readonly (Rectangle, int) MoveUpDoorZone = (new Rectangle(new Point(128, 576), new Point(64, 192)), 3);
    private static readonly (Rectangle, int) StopZone = (new Rectangle(new Point(128, 512), new Point(64, 64)), 4);

    // variables
    private bool reachedDoor;
    private string currentRoomSprite;
    private int MaxIntersectionArea;
    private ICommand loadRoomCommand;
    private Direction movementDirection;
    private IDungeonRoom startRoom;
    private Rectangle WallMasterRectangle;
    private SpriteDict FakeLink;
    private SpriteDict FakeWallMaster;
    private SpriteDict FakeBackground;
    private SpriteDict FakeBorder;
    private BlankSprite LeftCurtain;
    private BlankSprite RightCurtain;
    private GraphicsDevice graphicsDevice;

    // Add a Texture2D for drawing rectangles
    private Texture2D rectangleTexture;

    // Direction Map
    private static readonly Dictionary<(Rectangle,int), Direction> DirectionMap = new()
    {
        {MoveDownDoorZone,Direction.Up},
        {MoveUpDoorZone,Direction.Down},
        {MoveLeftZoneBottom,Direction.Right},
        {MoveLeftZoneTop,Direction.Right},
        {MoveDownZone,Direction.Up},
        {MoveUpZone,Direction.Down},
    };
    
    public WallMasterGrabScene(string currentRoomSprite, IDungeonRoom startRoom, ICommand loadRoomCommand, GraphicsDevice graphicsDevice)
    {
        this.startRoom = startRoom;
        this.currentRoomSprite = currentRoomSprite;
        this.loadRoomCommand = loadRoomCommand;
        this.graphicsDevice = graphicsDevice;
    }

    private void CreateFakeDoors(IDungeonRoom room)
    {
        foreach (var door in room.GetDoors())
        {
            var doorSprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 2, door.Position);
            doorSprite.SetSprite(door.Type.ToString());
        }
    }

    private void UpdateDirection()
    {
        // initialize priority value; first iteration should set actual priority value for comparison
        int priority = 100;

        // iteration over all zones and find which zone player is in
        foreach (var testZone in DirectionMap.Keys)
        {
            Rectangle zoneRectangle = testZone.Item1;
            if (zoneRectangle.Intersects(WallMasterRectangle))
            {
                int zonePriority = testZone.Item2;
                if (zonePriority < priority)
                {
                    priority = zonePriority;
                    movementDirection = DirectionMap[testZone];
                }
            }
        }

    }

    public override void LoadContent(ContentManager contentManager)
    {
        // make curtains
        var position = PlayerState.Position;
        var curtainSize = new Point(512, 704);
        var Center = new Point(graphicsDevice.Viewport.Width / 2, 192);
        var leftPosition = Center - new Point(graphicsDevice.Viewport.Width / 2, 0);
        LeftCurtain = new BlankSprite(SpriteLayer.HUD + 1, leftPosition, curtainSize, Color.Black);
        RightCurtain = new BlankSprite(SpriteLayer.HUD + 1, Center, curtainSize, Color.Black);
        LeftCurtain.Enabled = false;
        RightCurtain.Enabled = false;

        // create fake background, link ,and wallmaster
        FakeWallMaster = new SpriteDict(SpriteType.Enemies, SpriteLayer.HUD, position);
        FakeWallMaster.SetSprite("wallmaster");
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.HUD - 1, position);
        FakeLink.SetSprite("standing_down");
        FakeBackground = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 2, DungeonConstants.BackgroundPosition);
        FakeBackground.SetSprite(currentRoomSprite);

        // create FakeWallMaster rectangle
        WallMasterRectangle = new Rectangle(new Point(position.X - 32, position.Y - 32), new Point(52, 52));

        // Create a 1x1 white texture for rectangle drawing
        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { Color.White });
    }

    public override void Update(GameTime gameTime)
    {
        if(StopZone.Item1.Contains(WallMasterRectangle) == false)
        {
            UpdateDirection();
            Vector2 subtractionVector = 4 * DungeonConstants.DirectionVector[movementDirection];
            FakeLink.Position -= subtractionVector.ToPoint();
            FakeWallMaster.Position -= subtractionVector.ToPoint(); 
            WallMasterRectangle.Location -= subtractionVector.ToPoint();
        }
        else
        {
            FakeLink.Unregister();
            FakeWallMaster.Unregister();
            LeftCurtain.Enabled = true;
            RightCurtain.Enabled = true;
            FakeBackground.SetSprite(startRoom.RoomSprite.ToString());
            CreateFakeDoors(startRoom);
            if (RightCurtain.Position.X != graphicsDevice.Viewport.Width)
            {
                LeftCurtain.Position += new Point(-4, 0);
                RightCurtain.Position += new Point(4, 0);
            }
            else
            {
                FakeBackground.Unregister();
                PlayerState.Position = new Point(500, 725);
                loadRoomCommand.Execute(startRoom.RoomName);
            }
        }
    }
}
