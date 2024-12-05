using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Commands;
using MonoZelda.Sprites;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Items;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Sound;
using System.Collections.Generic;
using MonoZelda.Doors;

namespace MonoZelda.Scenes;

public class EnterInfiniteModeScene : Scene
{
    // string constants
    private const float INVENTORY_TOGGLE_TIME = 0.5f;
    private const string ENTRANCE_MESSAGE = "ENTER DOOR TO START";
    private const string UTILITY_MESSAGE = "STARTING ITEMS";
    private static readonly Vector2 ENTRANCE_MESSAGE_POINT = new Vector2(260,412);
    private static readonly Vector2 UTILITY_MESSAGE_POINT = new Vector2(328,448);

    // variables
    private string roomName;
    private bool pauseState;
    private float inventoryToggleTimer;
    private RoomGenerator roomGenerator;
    private SpriteFont spriteFont;
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager playerSprite;
    private EquippableManager equippableManager;
    private PlayerCollisionManager playerCollision;
    private ItemManager itemManager;
    private CollisionController collisionController;
    private ItemFactory itemFactory;
    private ICommand transitionCommand;
    private IDungeonRoom room;
    private List<IGameUpdate> updateables;

    public EnterInfiniteModeScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room)
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;
        updateables = new List<IGameUpdate>();

        // start here
        inventoryToggleTimer = INVENTORY_TOGGLE_TIME;
        pauseState = false;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Load SprintFont and calculate center for game over text
        spriteFont ??= contentManager.Load<SpriteFont>("Fonts/Basic");

        // Need to wait for LoadContent because MonoZeldaGame is going to clear everything before calling this.
        LoadRoom(contentManager);

        // Load Player and commands
        LoadPlayer();
        LoadCommands();

        // play fire sound effect
        SoundManager.PlaySound("LOZ_Campfire", true);
    }

    private void LoadCommands()
    {
        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.QuickSaveCommand, new EmptyCommand());
        commandManager.ReplaceCommand(CommandType.QuickLoadCommand, new EmptyCommand());
    }

    private void LoadPlayer()
    {
        // create player sprite classes
        playerSprite = new PlayerSpriteManager();
        var playerSpriteDict = new SpriteDict(SpriteType.Player, SpriteLayer.Player, PlayerState.Position);
        playerSprite.SetPlayerSpriteDict(playerSpriteDict);

        //create player and player collision manager
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50));
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollisionManager(playerSprite, playerHitbox, collisionController, null, null);

        // create itemFactory and spawn Items
        itemManager = new ItemManager(GameType.Infinite, null, room.GetItemSpawns(), null, playerCollision);
        itemFactory = new ItemFactory(collisionController, itemManager);
        SpawnItems();
    }

    private void LoadRoom(ContentManager contentManager)
    {
        transitionCommand = commandManager.GetCommand(CommandType.RoomTransitionCommand);

        LoadRoomTextures(contentManager);
        CreateStaticColliders();
    }

    private void SpawnItems()
    {
        itemFactory.CreateRoomItems();
    }

    private void CreateStaticColliders()
    {
        var roomColliderRects = room.GetStaticRoomColliders();
        foreach (var rect in roomColliderRects)
        {
            var collidable = new StaticRoomCollidable(rect);
            collisionController.AddCollidable(collidable);
        }
        var boundaryColliderRects = room.GetStaticBoundaryColliders();
        foreach (var rect in boundaryColliderRects)
        {
            var collidable = new StaticBoundaryCollidable(rect);
            collisionController.AddCollidable(collidable);
        }
    }

    private void LoadRoomTextures(ContentManager contentManager)
    {
        // Room wall border
        var r = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition);
        r.SetSprite(nameof(Dungeon1Sprite.room_exterior));
        r.Position = DungeonConstants.DungeonPosition;

        // Floor background
        var f = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.BackgroundPosition);
        f.SetSprite(room.RoomSprite.ToString());

        // Doors
        var doors = room.GetDoors();
        foreach (var door in doors)
        {
            var gameDoor = DoorFactory.CreateDoor(door, transitionCommand, collisionController, null);
        }
    }

    public override void UnloadContent()
    {
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand());
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand());
        base.UnloadContent();
    }

    public override void SetPaused(bool paused)
    {
        if (paused)
        {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new NavigableGridMoveCommand(PlayerState.EquippableManager));
        }
        else
        {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new EmptyCommand());
        }

        // update pauseState
        inventoryToggleTimer = INVENTORY_TOGGLE_TIME;
        pauseState = paused;
    }

    public override void Draw(SpriteBatch batch)
    {
        inventoryToggleTimer -= (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;

        if(pauseState == false && inventoryToggleTimer < 0)
        {
            batch.DrawString(spriteFont, ENTRANCE_MESSAGE, ENTRANCE_MESSAGE_POINT, Color.White);
            batch.DrawString(spriteFont, UTILITY_MESSAGE, UTILITY_MESSAGE_POINT, Color.White);
        }
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var updateable in updateables)
        {
            updateable.Update(gameTime);
        }

        PlayerState.EquippableManager.Update();
        itemManager.Update();
        playerCollision.Update();
    }
}
