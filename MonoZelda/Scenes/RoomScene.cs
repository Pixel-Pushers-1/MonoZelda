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
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies.EnemyClasses;
using MonoZelda.Trigger;
using MonoZelda.Sound;
using MonoZelda.Tiles.Doors;

namespace MonoZelda.Scenes;

public class RoomScene : Scene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager playerSprite;
    private ProjectileManager projectileManager;
    private PlayerCollisionManager playerCollision;
    private CollisionController collisionController;
    private List<IGameUpdate> updateables;
    private ItemFactory itemFactory;
    private EnemyFactory enemyFactory;
    private List<IEnemy> enemies = new();
    private Dictionary<IEnemy, EnemyCollisionManager> enemyDictionary = new();
    private List<EnemyCollisionManager> enemyCollisions = new();
    private List<EnemyProjectileCollisionManager> enemyProjectileCollisions = new();
    private IDungeonRoom room;
    private string roomName;

    public RoomScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room) 
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;
        updateables = new List<IGameUpdate>();
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Need to wait for LoadContent because MonoZeldaGame is going to clear everything before calling this.
        LoadRoom(contentManager);

        // create player sprite classes
        playerSprite = new PlayerSpriteManager();
        var playerSpriteDict = new SpriteDict(SpriteType.Player, SpriteLayer.Player, PlayerState.Position);
        playerSprite.SetPlayerSpriteDict(playerSpriteDict);

        // Play Dungeon Theme
        SoundManager.PlaySound("LOZ_Dungeon_Theme", true);

        //create player and player collision manager
        var takeDamageCommand = new PlayerTakeDamageCommand(playerSprite);
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50));
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollisionManager(playerSprite, playerHitbox, collisionController, takeDamageCommand);

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(SpriteType.Projectiles, 0, new Point(0, 0));
        projectileManager = new ProjectileManager(collisionController, projectileDict);

        // Create itemFactory and HUDManager
        itemFactory = new ItemFactory(collisionController);

        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerEquipProjectileCommand, new PlayerEquipProjectileCommand(projectileManager));   
        commandManager.ReplaceCommand(CommandType.PlayerFireSwordBeamCommand, new PlayerFireSwordBeamCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new PlayerFireProjectileCommand(projectileManager, playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(playerSprite));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerSprite));
    }

    private void LoadRoom(ContentManager contentManager)
    {
        LoadRoomTextures(contentManager);
        CreateStaticColliders();
        CreateTriggers(contentManager);
        SpawnItems(contentManager);
        SpawnEnemies(contentManager);
        LoadDoors();
    }

    private void CreateTriggers(ContentManager contentManager)
    {
        foreach(var trigger in room.GetTriggers())
        {
            var t = TriggerFactory.CreateTrigger(trigger.Type, collisionController, trigger.Position);
            if(t is IGameUpdate updateable)
            {
                updateables.Add(updateable);
            }
        }
    }

    private void SpawnItems(ContentManager contentManager)
    {
        // Create itemFactory object
        itemFactory = new ItemFactory(collisionController);
        foreach (var itemSpawn in room.GetItemSpawns())
        {
            itemFactory.CreateItem(itemSpawn.ItemType, itemSpawn.Position);
        }
    }

    private void SpawnEnemies(ContentManager contentManager)
    {
        enemyFactory = new EnemyFactory(collisionController);
        foreach(var enemySpawn in room.GetEnemySpawns())
        {
            enemies.Add(enemyFactory.CreateEnemy(enemySpawn.EnemyType, new Point(enemySpawn.Position.X + 32, enemySpawn.Position.Y + 32)));
        }
        foreach (var enemy in enemies)
        {
            enemyDictionary.Add(enemy, new EnemyCollisionManager(enemy, collisionController, enemy.Width, enemy.Height));
        }
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

        
    }

    private void LoadDoors()
    {
        var doors = room.GetDoors();
        foreach (var door in doors)
        {
            var transitionCommand = commandManager.GetCommand(CommandType.RoomTransitionCommand);

            var dDoor = DoorFactory.CreateDoor(door, transitionCommand, collisionController, enemies);
            if (dDoor is IGameUpdate updateable)
            {
                updateables.Add(updateable);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (projectileManager.ProjectileFired == true)
        {
            projectileManager.UpdatedProjectileState();
        }

        foreach(KeyValuePair<IEnemy, EnemyCollisionManager> entry in enemyDictionary)
        {
            if (!entry.Key.Alive) // remove dead enemies from lists (hopefully this is useful for re-entering rooms)
            {
                enemies.Remove(entry.Key);
                enemyDictionary.Remove(entry.Key);
            }
            entry.Key.Update(gameTime);
            if (entry.Key.GetType() == typeof(Aquamentus))
            {
                entry.Value.Update(entry.Key.Width, entry.Key.Height, new Point(entry.Key.Pos.X - 16, entry.Key.Pos.Y - 16));
            }
            else
            {
                entry.Value.Update(entry.Key.Width, entry.Key.Height, entry.Key.Pos);
            }
        }
        
        foreach (var updateable in updateables)
        {
            updateable.Update(gameTime);
        }

        playerCollision.Update();
    }
}
