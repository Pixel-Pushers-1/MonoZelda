using Microsoft.Xna.Framework.Content;
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
    private static readonly Point KEY_POSITION = new Point(640, 704);
    private static readonly (Rectangle, int) MoveUpZone = (new Rectangle(new Point(832, 384), new Point(64, 160)), 1);
    private static readonly (Rectangle, int) MoveDownZone = (new Rectangle(new Point(832, 544), new Point(64, 160)), 1);
    private static readonly (Rectangle, int) MoveLeftZoneTop = (new Rectangle(new Point(192, 320), new Point(704, 64)), 2);
    private static readonly (Rectangle, int) MoveLeftZoneBottom = (new Rectangle(new Point(192, 704), new Point(704, 64)), 2);
    private static readonly (Rectangle, int) MoveDownDoorZone = (new Rectangle(new Point(128, 320), new Point(64, 192)), 3);
    private static readonly (Rectangle, int) MoveUpDoorZone = (new Rectangle(new Point(128, 576), new Point(64, 192)), 3);
    private static readonly (Rectangle, int) StopZone = (new Rectangle(new Point(128, 512), new Point(64, 64)), 4);

    // variables
    private bool reachedDoor;
    private float stopZoneTimer;
    private int MaxIntersectionArea;
    private IDungeonRoom currentRoom;
    private ICommand enterDungeonAnimationCommand;
    private Direction movementDirection;
    private IDungeonRoom startRoom;
    private Rectangle WallMasterRectangle;
    private SpriteDict FakeKey;
    private SpriteDict FakeLink;
    private SpriteDict FakeWallMaster;
    private SpriteDict FakeBackground;
    private SpriteDict FakeBorder;

    // Add a Texture2D for drawing rectangles
    private Texture2D rectangleTexture;

    // Direction Map
    private static readonly Dictionary<(Rectangle, int), Direction> DirectionMap = new()
    {
        {MoveDownDoorZone,Direction.Up},
        {MoveUpDoorZone,Direction.Down},
        {MoveLeftZoneBottom,Direction.Right},
        {MoveLeftZoneTop,Direction.Right},
        {MoveDownZone,Direction.Up},
        {MoveUpZone,Direction.Down},
    };

    public WallMasterGrabScene(IDungeonRoom currentRoom, IDungeonRoom startRoom, ICommand enterDungeonAnimationCommand)
    {
        stopZoneTimer = 0.5f;
        this.startRoom = startRoom;
        this.currentRoom = currentRoom;
        this.enterDungeonAnimationCommand = enterDungeonAnimationCommand;
    }

    private void CreateFakeDoors(IDungeonRoom room)
    {
        foreach (var door in room.GetDoors())
        {
            var doorSprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 1, door.Position);
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
        // get position of link
        var position = PlayerState.Position;

        // create fake link ,and wallmaster
        FakeWallMaster = new SpriteDict(SpriteType.Enemies, SpriteLayer.HUD, position);
        FakeWallMaster.SetSprite("wallmaster");
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.HUD - 1, position);
        FakeLink.SetSprite("standing_down");
        FakeKey = new SpriteDict(SpriteType.Items, SpriteLayer.HUD - 1, KEY_POSITION);
        FakeKey.SetSprite("key_0");

        // create fake doors, background and roomborder
        FakeBackground = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 2, DungeonConstants.BackgroundPosition);
        FakeBackground.SetSprite(currentRoom.RoomSprite.ToString());
        FakeBorder = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 2, DungeonConstants.DungeonPosition);
        FakeBorder.SetSprite("room_exterior");
        CreateFakeDoors(currentRoom);

        // create FakeWallMaster rectangle
        WallMasterRectangle = new Rectangle(new Point(position.X - 32, position.Y - 32), new Point(52, 52));
    }

    public override void Update(GameTime gameTime)
    {
        if (StopZone.Item1.Contains(WallMasterRectangle) == false)
        {
            UpdateDirection();
            Vector2 subtractionVector = 4 * DungeonConstants.DirectionVector[movementDirection];
            FakeLink.Position -= subtractionVector.ToPoint();
            FakeWallMaster.Position -= subtractionVector.ToPoint();
            WallMasterRectangle.Location -= subtractionVector.ToPoint();
        }
        else
        {
            FakeBackground.Unregister();
            FakeLink.Unregister();
            FakeWallMaster.Unregister();
            FakeKey.Unregister();
            FakeBackground.Unregister();
            PlayerState.Position = new Point(515, 725);
            enterDungeonAnimationCommand.Execute(startRoom);
        }
    }
}
