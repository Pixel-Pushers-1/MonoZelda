using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Enemies;
using MonoZelda.Items;
using MonoZelda.Link;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using MonoZelda.Tiles.Doors;
using MonoZelda.Trigger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoZelda.Scenes.InfiniteMode;

public class InfiniteRoomScene : Scene
{
    private string roomName;
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager playerSprite;
    private EquippableManager equippableManager;
    private PlayerCollisionManager playerCollision;
    private ItemManager itemManager;
    private CollisionController collisionController;
    private ItemFactory itemFactory;
    private EnemyFactory enemyFactory;
    private List<Enemy> enemies = new();
    private Dictionary<Enemy, EnemySpawn> enemySpawnPoints = new();
    private List<IGameUpdate> updateables = new();
    private List<IDisposable> disposables = new();
    private ICommand transitionCommand;
    private IDungeonRoom room;

    // start here
    private RoomGenerator randomRoomGenerator;

    public InfiniteRoomScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room)
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;

        // start here
       
        randomRoomGenerator = new RoomGenerator();
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Need to wait for LoadContent because MonoZeldaGame is going to clear everything before calling this.
        LoadRoom(contentManager);

        LoadPlayer();
        LoadCommands();
    }

    protected void LoadCommands()
    {
        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(equippableManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerCycleEquippableCommand, new PlayerCycleEquippableCommand(equippableManager));
        commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(equippableManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerSprite));
    }

    protected void LoadPlayer()
    {
        // create player sprite classes
        playerSprite = new PlayerSpriteManager();
        var playerSpriteDict = new SpriteDict(SpriteType.Player, SpriteLayer.Player, PlayerState.Position);
        playerSprite.SetPlayerSpriteDict(playerSpriteDict);

        //create player and player collision manager
        var linkDeathAnimationCommand = commandManager.GetCommand(CommandType.LinkDeathAnimationCommand);
        var takeDamageCommand = new PlayerTakeDamageCommand(playerSprite);
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50));
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollisionManager(playerSprite, playerHitbox, collisionController, linkDeathAnimationCommand, takeDamageCommand);

        // create itemFactory and spawn Items
        var levelCompleteAnimationCommand = commandManager.GetCommand(CommandType.LevelCompleteAnimationCommand);
        itemManager = new ItemManager(GameType.infiniteEasy, levelCompleteAnimationCommand, room.GetItemSpawns(), enemies, playerCollision);
        itemFactory = new ItemFactory(collisionController, itemManager);
        SpawnItems();

        // spawnEnemies
        SpawnEnemies();

        // create equippableManager
        equippableManager = new EquippableManager(collisionController);
    }

    private void LoadRoom(ContentManager contentManager)
    {
        transitionCommand = commandManager.GetCommand(CommandType.RoomTransitionCommand);

        LoadRoomTextures(contentManager);
        CreateStaticColliders();
    }

    protected void SpawnItems()
    {
        //itemFactory.CreateRoomItems();
    }

    protected void SpawnEnemies()
    {
        //enemyFactory = new EnemyFactory(collisionController);
        //foreach (var enemySpawn in room.GetEnemySpawns())
        //{
        //    var enemy = enemyFactory.CreateEnemy(enemySpawn.EnemyType,
        //        new Point(enemySpawn.Position.X + 32, enemySpawn.Position.Y + 32), itemFactory, enemySpawn.HasKey);
        //    enemies.Add(enemy);
        //    enemySpawnPoints.Add(enemy, enemySpawn);
        //}
    }

    protected void CreateStaticColliders()
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

        // Create the single room door
        var doors = room.GetDoors();
        foreach(var door in doors)
        {
            if(door.Destination != null)
            {
                door.Destination = randomRoomGenerator.GetRoom();
            }
            var gameDoor = DoorFactory.CreateDoor(door, transitionCommand, collisionController, enemies);
            if (gameDoor is IGameUpdate updateable)
            {
                updateables.Add(updateable);
            }
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
        }
        else
        {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(equippableManager, playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(equippableManager, playerSprite));
        }

        // allow cycling of items since game is paused
        equippableManager.IsPaused = paused;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var enemy in enemies.ToList())
        {
            if (!enemy.Alive)
            {
                room.Remove(enemySpawnPoints[enemy]);
                var itemRooms = new List<string> { "Room16", "Room12", "Room2", "Room5" };
                enemies.Remove(enemy);
                if (enemies.Count == 0 && itemRooms.Contains(room.RoomName))
                {
                    if (room.RoomName == "Room12")
                    {
                        itemFactory.CreateItem(new ItemSpawn(new Point(500, 400), ItemList.Boomerang), true);
                    }
                    else
                    {
                        itemFactory.CreateItem(new ItemSpawn(new Point(500, 400), ItemList.Key), true);
                    }
                    SoundManager.PlaySound("LOZ_Key_Appear", false);
                }
            }
            enemy.Update();
        }

        foreach (var updateable in updateables)
        {
            updateable.Update(gameTime);
        }

        equippableManager.Update();
        itemManager.Update();
        playerCollision.Update();
    }
}
