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
using MonoZelda.Enemies;
using System.Collections.Generic;
using System.Linq;
using MonoZelda.Trigger;
using MonoZelda.Sound;
using MonoZelda.Shaders;
using MonoZelda.Doors;

namespace MonoZelda.Scenes;

public class RoomScene : Scene
{
    // constants
    private const int DEFAULT_ENEMY_lEVEL = 1;

    // variables
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager playerSprite;
    private PlayerCollisionManager playerCollision;
    private ItemManager itemManager;
    private ICommand transitionCommand;
    private CollisionController collisionController;
    private List<ITrigger> triggers;
    private ItemFactory itemFactory;
    private EnemyFactory enemyFactory;
    private List<Enemy> enemies = new();
    private Dictionary<Enemy, EnemySpawn> enemySpawnPoints = new();
    private IDungeonRoom room;
    private string roomName;
    private List<ILight> lights = new();
    private ILight playerLight; 
    private List<IGameUpdate> updateables = new();
    private List<IDisposable> disposables = new();

    public RoomScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room) 
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;
        triggers = new List<ITrigger>();
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Need to wait for LoadContent because MonoZeldaGame is going to clear everything before calling this.
        LoadRoom(contentManager);

        // Load Player and commands
        LoadPlayer();
        LoadCommands();
    }

    protected void LoadCommands()
    {
        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerSprite));
    }

    protected void LoadPlayer()
    {
        // create player sprite classes
        playerSprite = new PlayerSpriteManager();
        var playerSpriteDict = new SpriteDict(SpriteType.Player, SpriteLayer.Player, PlayerState.Position);
        playerSprite.SetPlayerSpriteDict(playerSpriteDict);
        PlayerState.ResetCandle();

        //create player and player collision manager
        var linkDeathAnimationCommand = commandManager.GetCommand(CommandType.LinkDeathAnimationCommand);
        var takeDamageCommand = new PlayerTakeDamageCommand(playerSprite);
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50));
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollisionManager(playerSprite, playerHitbox, collisionController, linkDeathAnimationCommand, takeDamageCommand);

        // create itemFactory and spawn Items
        var levelCompleteAnimationCommand = commandManager.GetCommand(CommandType.LevelCompleteAnimationCommand);
        itemManager = new ItemManager(GameType.Infinite, levelCompleteAnimationCommand, room.GetItemSpawns(), enemies, playerCollision);
        itemFactory = new ItemFactory(collisionController, itemManager);
        SpawnItems();

        // spawnEnemies
        SpawnEnemies();

        // Play Dungeon Theme
        SoundManager.PlaySound("LOZ_Dungeon_Theme", true);
    }

    private void LoadRoom(ContentManager contentManager)
    {
        transitionCommand = commandManager.GetCommand(CommandType.RoomTransitionCommand);
        
        SetupShader();
        CreateTriggers(contentManager);
        LoadRoomTextures(contentManager);
        CreateStaticColliders();
    }

    private void SetupShader()
    {
        // set list of lights in equippableManager
        PlayerState.EquippableManager.Lights = lights;

        if(room.IsLit)
        {
            playerLight = new PlayerLight();
            lights.Add(playerLight);

            // Demo lights
            // TODO: Light emmitting items
            // Randomly load left or right light based on roomname for determinizim

            var random = new Random(room.RoomName.GetHashCode());
            var lightIndex = random.Next(0, 3);

            if (lightIndex == 0)
            {
                lights.Add(new Light() {
                    Position = new Point(250, 256),
                    Color = Color.Orange,
                    Radius = 400,
                    Intensity = 0.75f
                });
            }
            if(lightIndex == 1)
            {
                lights.Add(new Light() {
                    Position = new Point(774, 256),
                    Color = Color.Orange,
                    Radius = 400,
                    Intensity = 0.75f
                });
            }    

            var intersectors = new List<Vector4>(256);
            var roomColliderRects = room.GetStaticRoomColliders();
            var height = graphicsDevice.Viewport.Height;
            foreach (var rect in roomColliderRects)
            {
                intersectors.Add(new Vector4(rect.X, height - rect.Y, rect.Width, rect.Height)); // left
            }

            // Limited to 75 line segments
            var arrayIntersectors = intersectors.Take(CustomShader.MAX_LIGHT_COLLIDERS).ToArray();
            MonoZeldaGame.Shader.SetLineSegments(arrayIntersectors);
        }
    }

    private void CreateTriggers(ContentManager contentManager)
    {
        foreach(var trigger in room.GetTriggers())
        {
            var t = TriggerFactory.CreateTrigger(trigger, collisionController, transitionCommand);
            triggers.Add(t);

            if (t is IGameUpdate updateable)
            {
                updateables.Add(updateable);
            }
        }
    }

    protected void SpawnItems()
    {
        itemFactory.CreateRoomItems();
    }

    protected void SpawnEnemies()
    {
        MonoZeldaGame.EnemyLevel = DEFAULT_ENEMY_lEVEL;
        enemyFactory = new EnemyFactory(collisionController);
        foreach(var enemySpawn in room.GetEnemySpawns())
        {
            var enemy = enemyFactory.CreateEnemy(enemySpawn.EnemyType,
                new Point(enemySpawn.Position.X + 32, enemySpawn.Position.Y + 32), itemFactory,enemyFactory, enemySpawn.HasKey);
            enemies.Add(enemy);
            enemySpawnPoints.Add(enemy,enemySpawn);
        }
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

        // Doors
        var doors = room.GetDoors();
        foreach (var door in doors)
        {
            var gameDoor = DoorFactory.CreateDoor(door, transitionCommand, collisionController, enemies, triggers);
            if (gameDoor is IGameUpdate updateable)
            {
                updateables.Add(updateable);
            }
        }
    }

    public override void UnloadContent()
    {
        MonoZeldaGame.Shader.Reset();

        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand());
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand());
        base.UnloadContent();
    }

    public override void SetPaused(bool paused) 
    {
        if (paused) {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new NavigableGridMoveCommand(PlayerState.EquippableManager));
        }
        else {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new EmptyCommand());
        }
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
                        itemFactory.CreateItem(new ItemSpawn(new Point(500,400),ItemList.Boomerang),true);
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

        UpdateDynamicLights();

        PlayerState.EquippableManager.Update();
        itemManager.Update();
        playerCollision.Update();
    }

    private void UpdateDynamicLights()
    {
        if(!room.IsLit)
            return;

        MonoZeldaGame.Shader.SetDynamicLights(lights);
    }
}
