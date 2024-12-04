using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Commands;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Dungeon1;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.Scenes;

public class TransitionScene : Scene
{
    private const string RANDOM_ROOM = "RandomRoom";
    private ICommand loadCommand;
    private IDungeonRoom currentRoom;
    private IDungeonRoom nextRoom;
    private Direction TransitionDirection;
    private List<SpriteDict> spritesToMove;
    private SpriteDict FakeLink;
    private SpriteDict DoorLayer;
    private Vector2 FakeLinkPosition;
    private Vector2 InitialPosition;
    private Vector2 movement;
    private float FakeLinkSpeed = 4f;

    private readonly Dictionary<Direction, Point> directionShiftMap = new()
    {
        { Direction.Left, new Point(8,0) },
        { Direction.Right, new Point(-8,0) },
        { Direction.Up, new Point(0,8) },
        { Direction.Down, new Point(0,-8) },
    };

    private readonly Dictionary<Direction, (string,Direction)> DirectionMap = new()
    {
       { Direction.Up, ("down",Direction.Down) },
       { Direction.Down, ("up", Direction.Up) },
       { Direction.Left, ("right", Direction.Right) },
       { Direction.Right, ("left", Direction.Left) }
    };

    public TransitionScene(IDungeonRoom currentRoom, IDungeonRoom nextRoom, ICommand loadCommand, Direction transitionDirection)
    {
        this.loadCommand = loadCommand;
        this.currentRoom = currentRoom;
        this.nextRoom = nextRoom;
        TransitionDirection = transitionDirection;
        movement = DungeonConstants.TransitionDirectionVector[TransitionDirection];
        InitialPosition = DungeonConstants.TransitionLinkSpawnPoints[transitionDirection].ToVector2();
        spritesToMove = new List<SpriteDict>();
    }

    private void CreateSpriteDict(string spriteName, Point position, int priority = SpriteLayer.Background)
    {
        var spriteDict = new SpriteDict(SpriteType.Blocks, priority, position);
        spriteDict.SetSprite(spriteName);
        spritesToMove.Add(spriteDict);
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Set up room and border sprites
        CreateSpriteDict("room_exterior", DungeonConstants.DungeonPosition);
        CreateSpriteDict("room_exterior", DungeonConstants.DungeonPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);
        CreateSpriteDict(currentRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition);
        CreateSpriteDict(nextRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);

        // create Door spriteDicts
        foreach (var currentDoorSpawn in currentRoom.GetDoors())
        {
            if (currentRoom.RoomName == RANDOM_ROOM && currentDoorSpawn.Direction == DoorDirection.West)
            {
                CreateSpriteDict(Dungeon1Sprite.door_closed_west.ToString(), currentDoorSpawn.Position, SpriteLayer.DoorLayer);
            }
            else
            {
                CreateSpriteDict(currentDoorSpawn.Type.ToString(), currentDoorSpawn.Position, SpriteLayer.DoorLayer);
            }
        }

        foreach (var nextDoorSpawn in nextRoom.GetDoors())
        {
            // Apply KeyRing to doors
            if (PlayerState.Keyring.Contains((nextDoorSpawn.RoomName, nextDoorSpawn.Direction)))
            {
                nextDoorSpawn.Type = nextDoorSpawn.Type.ToOpened();
            }

            CreateSpriteDict(nextDoorSpawn.Type.ToString(), nextDoorSpawn.Position + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection], SpriteLayer.DoorLayer);
        }

        //Initialize Fake Link
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.HUD, DungeonConstants.TransitionLinkSpawnPoints[TransitionDirection]);
        FakeLink.SetSprite($"walk_{DirectionMap[TransitionDirection].Item1}");
        FakeLinkPosition = DungeonConstants.TransitionLinkSpawnPoints[TransitionDirection].ToVector2();
        FakeLink.Enabled = false;
    }

    public override void Update(GameTime gameTime)
    {
        Point shift = directionShiftMap[TransitionDirection];
        if (Vector2.Distance(spritesToMove[1].Position.ToVector2(), DungeonConstants.DungeonPosition.ToVector2()) > 0)
        {
            foreach (var spriteDict in spritesToMove)
            {
                spriteDict.Position -= shift;
            }
        }
        else
        {
            FakeLink.Enabled = true;
            if (Vector2.Distance(InitialPosition, FakeLinkPosition) < 64)
            {
                FakeLinkPosition += FakeLinkSpeed * movement;
                FakeLink.Position = FakeLinkPosition.ToPoint();
            }
            else
            {
                PlayerState.Position = FakeLinkPosition.ToPoint();
                PlayerState.Direction = DirectionMap[TransitionDirection].Item2;
                nextRoom.SpawnPoint = PlayerState.Position;
                loadCommand.Execute(nextRoom.RoomName);
            }
        }
    }
}