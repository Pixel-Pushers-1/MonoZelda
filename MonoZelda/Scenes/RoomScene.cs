using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Commands;
using MonoZelda.Sprites;
using MonoZelda.Link.Projectiles;
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
using MonoZelda.Tiles.Doors;
using MonoZelda.Dungeons.Dungeon1;
using MonoZelda.Shaders;
using System.Runtime.CompilerServices;

namespace MonoZelda.Scenes;

public class RoomScene : Scene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager playerSprite;
    private ProjectileManager projectileManager;
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

        // Play Dungeon Theme
        SoundManager.PlaySound("LOZ_Dungeon_Theme", true);

        LoadPlayer();
        LoadCommands();
    }

    protected void LoadCommands()
    {
        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerEquipProjectileCommand, new PlayerEquipProjectileCommand(projectileManager));   
        //commandManager.ReplaceCommand(CommandType.PlayerFireSwordBeamCommand, new PlayerFireSwordBeamCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new PlayerFireProjectileCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerSprite));
    }

    protected void LoadPlayer()
    {
        // create player sprite classes
        playerSprite = new PlayerSpriteManager();
        var playerSpriteDict = new SpriteDict(SpriteType.Player, SpriteLayer.Player, PlayerState.Position);
        playerSprite.SetPlayerSpriteDict(playerSpriteDict);

        //create player and player collision manager
        var takeDamageCommand = new PlayerTakeDamageCommand(playerSprite);
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50));
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollisionManager(playerSprite, playerHitbox, collisionController, takeDamageCommand);

        // create itemFactory and spawn Items
        itemManager = new ItemManager(room.GetItemSpawns(), enemies, playerCollision);
        itemFactory = new ItemFactory(collisionController, itemManager);
        SpawnItems();

        // spawnEnemies
        SpawnEnemies();

        // Play Dungeon Theme
        SoundManager.PlaySound("LOZ_Dungeon_Theme", true);

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(SpriteType.Projectiles, 0, new Point(0, 0));
        projectileManager = new ProjectileManager(collisionController, projectileDict);
    }

    private void LoadRoom(ContentManager contentManager)
    {
        transitionCommand = commandManager.GetCommand(CommandType.RoomTransitionCommand);
        
        SetupShader();
        LoadRoomTextures(contentManager);
        CreateStaticColliders();
        CreateTriggers(contentManager);
    }

    private void SetupShader()
    {
        if(room.IsLit)
        {
            playerLight = new PlayerLight();
            lights.Add(playerLight);

            // Demo lights
            // TODO: Light emmitting items
            lights.Add(new Light() {
                Position = new Point(250, 256),
                Color = Color.Orange,
                Radius = 400,
                Intensity = 0.75f
            });

            lights.Add(new Light() {
                Position = new Point(774, 256),
                Color = Color.Orange,
                Radius = 400,
                Intensity = 0.75f
            });

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
        enemyFactory = new EnemyFactory(collisionController);
        foreach(var enemySpawn in room.GetEnemySpawns())
        {
            var enemy = enemyFactory.CreateEnemy(enemySpawn.EnemyType,
                new Point(enemySpawn.Position.X + 32, enemySpawn.Position.Y + 32), itemFactory, enemySpawn.HasKey);
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
            var gameDoor = DoorFactory.CreateDoor(door, transitionCommand, collisionController, enemies);
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

    public void SetPaused(bool paused) {
        if (paused) {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new EmptyCommand());
            commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new EmptyCommand());
        }
        else {
            commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(projectileManager, playerSprite));
            commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new PlayerFireProjectileCommand(projectileManager, playerSprite));
        }

    }

    public override void Update(GameTime gameTime)
    {
        if (projectileManager.ProjectileFired == true)
        {
            projectileManager.UpdatedProjectileState();
        }

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
