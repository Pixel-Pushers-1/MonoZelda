using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Dungeons;
using System.Collections.Generic;
using MonoZelda.Commands;
using MonoZelda.Sound;

namespace MonoZelda.Scenes;

public class WallMasterGrabScene : Scene
{
    // movement zones
    private static readonly Rectangle MoveLeftZoneTop = new Rectangle(new Point(192, 320), new Point(704, 64));
    private static readonly Rectangle MoveLeftZoneBottom = new Rectangle(new Point(192, 704), new Point(704, 64));
    private static readonly Rectangle MoveDownDoorZone = new Rectangle(new Point(128, 320), new Point(64, 192));
    private static readonly Rectangle MoveUpDoorZone = new Rectangle(new Point(128, 576), new Point(64, 192));
    private static readonly Rectangle MoveUpZone = new Rectangle(new Point(832, 384), new Point(64, 160));
    private static readonly Rectangle MoveDownZone = new Rectangle(new Point(832, 544), new Point(64, 160));
    private static readonly Rectangle StopZone = new Rectangle(new Point(128, 512), new Point(64, 64));

    // variables
    private bool reachedDoor;
    private string currentRoomSprite;
    private ICommand loadRoomCommand;
    private Direction movementDirection;
    private IDungeonRoom startRoom;
    private Rectangle WallMasterRectangle;
    private SpriteDict FakeLink;
    private SpriteDict FakeWallMaster;
    private SpriteDict FakeBackground;
    private BlankSprite LeftCurtain;
    private BlankSprite RightCurtain;
    private GraphicsDevice graphicsDevice;

    // Direction Map
    private static readonly Dictionary<Rectangle, Direction> DirectionMap = new()
    {
        {MoveDownDoorZone,Direction.Down},
        {MoveUpDoorZone,Direction.Up},
        {MoveLeftZoneBottom,Direction.Left},
        {MoveLeftZoneTop,Direction.Left},
        {MoveDownZone,Direction.Down},
        {MoveUpZone,Direction.Up},
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
        foreach (var movementZone in DirectionMap.Keys)
        {
            if (movementZone.Intersects(WallMasterRectangle))
            {
                if (movementZone.Contains(WallMasterRectangle))
                {
                    movementDirection = DirectionMap[movementZone];
                    return;
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
        LeftCurtain = new BlankSprite(SpriteLayer.HUD + 1, Center, curtainSize, Color.Black);
        RightCurtain = new BlankSprite(SpriteLayer.HUD + 1, Center, curtainSize, Color.Black);
        LeftCurtain.Enabled = true;
        RightCurtain.Enabled = true;

        // create fake background, link ,and wallmaster
        FakeWallMaster = new SpriteDict(SpriteType.Enemies, SpriteLayer.HUD, position);
        FakeWallMaster.SetSprite("wallmaster");
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.HUD - 1, position);
        FakeLink.SetSprite("standing_down");
        FakeBackground = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD - 2, DungeonConstants.BackgroundPosition);
        FakeBackground.SetSprite(currentRoomSprite);

        // create FakeWallMaster rectangle
        WallMasterRectangle = new Rectangle(new Point(position.X - 32, position.Y - 32), new Point(52, 52));
    }

    public override void Update(GameTime gameTime)
    {
        if(StopZone.Contains(WallMasterRectangle) == false)
        {
            UpdateDirection();
            Vector2 subtractionVector = 4 * DungeonConstants.DirectionVector[movementDirection];
            FakeLink.Position -= subtractionVector.ToPoint();
            FakeWallMaster.Position -= subtractionVector.ToPoint(); 
            WallMasterRectangle.Location -= subtractionVector.ToPoint();
        }
        else
        {
            LeftCurtain.Enabled = true;
            RightCurtain.Enabled = true;
            FakeBackground.SetSprite(startRoom.RoomSprite.ToString());
            CreateFakeDoors(startRoom);
            if (LeftCurtain.Position.X != 0)
            {
                LeftCurtain.Position += new Point(4, 0);
                RightCurtain.Position += new Point(-4, 0);
            }
            else
            {
                FakeBackground.Unregister();
                FakeLink.Unregister();
                FakeWallMaster.Unregister();
                PlayerState.Position = new Point(500, 725);
                loadRoomCommand.Execute(startRoom.RoomName);
            }
        }
    }
}
