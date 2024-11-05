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
using MonoZelda.HUD;

namespace MonoZelda.Scenes;

public class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private PlayerSpriteManager player;
    private ProjectileManager projectileManager;
    private PlayerCollisionManager playerCollision;
    private CollisionController collisionController;
    private List<ITrigger> triggers;
    private ItemFactory itemFactory;
    private EnemyFactory enemyFactory;
    private List<IEnemy> enemies = new();
    private Dictionary<IEnemy, EnemyCollisionManager> enemyDictionary = new();
    private List<EnemyCollisionManager> enemyCollisions = new();
    private List<EnemyProjectileCollisionManager> enemyProjectileCollisions = new();
    private IDungeonRoom room;
    private string roomName;
    private HUDManager hudManager;
    private PlayerState playerState;


    public DungeonScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room) 
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;
        triggers = new List<ITrigger>();
    }

    public void LoadContent(ContentManager contentManager)
    {
        // Need to wait for LoadContent because MonoZeldaGame is going to clear everything before calling this.
        LoadRoom(contentManager);


        //create playerState and player collisionManager and playerSpriteDrawer
        player = new PlayerSpriteManager(playerState);
        PlayerCollidable playerHitbox = new PlayerCollidable(new Rectangle(100, 100, 50, 50), graphicsDevice);
        collisionController.AddCollidable(playerHitbox);
        playerState = new PlayerState(player);
        playerCollision = new PlayerCollisionManager(player, playerHitbox, collisionController, playerState);
        playerState = new PlayerState(player);
        playerCollision = new PlayerCollisionManager(player, playerHitbox, collisionController, playerState);

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(SpriteType.Projectiles, 0, new Point(0, 0));
        projectileManager = new ProjectileManager(collisionController, graphicsDevice, projectileDict);

        // Create itemFactory and HUDManager
        itemFactory = new ItemFactory(collisionController, contentManager, graphicsDevice);
        hudManager = new HUDManager(contentManager, projectileManager);

        //
        playerState.HUDManager = hudManager;
        hudManager.PlayerState = playerState;

        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerEquipProjectileCommand, new PlayerEquipProjectileCommand(projectileManager, hudManager));   
        commandManager.ReplaceCommand(CommandType.PlayerFireSwordBeamCommand, new PlayerFireSwordBeamCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new PlayerFireProjectileCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(playerState));

        // create spritedict to pass into player controller
        var playerSpriteDict = new SpriteDict(SpriteType.Player, 1, new Point(100, 100));
        player.SetPlayerSpriteDict(playerSpriteDict);
       
    }
    private void LoadRoom(ContentManager contentManager)
    {
        LoadRoomTextures(contentManager);
        CreateStaticColliders();
        CreateTriggers(contentManager);
        SpawnItems(contentManager);
        SpawnEnemies(contentManager);
    }

    private void CreateTriggers(ContentManager contentManager)
    {
        foreach(var trigger in room.GetTriggers())
        {
            var t = TriggerFactory.CreateTrigger(trigger.Type, collisionController, contentManager, trigger.Position, graphicsDevice);
            triggers.Add(t);
        }
    }

    private void SpawnItems(ContentManager contentManager)
    {
        // Create itemFactory object
        itemFactory = new ItemFactory(collisionController, contentManager, graphicsDevice);
        foreach (var itemSpawn in room.GetItemSpawns())
        {
            itemFactory.CreateItem(itemSpawn.ItemType, itemSpawn.Position);
        }
    }

    private void SpawnEnemies(ContentManager contentManager)
    {
        enemyFactory = new EnemyFactory(collisionController, contentManager, graphicsDevice);
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
            var collidable = new StaticRoomCollidable(rect, graphicsDevice);
            collisionController.AddCollidable(collidable);
        }
        var boundaryColliderRects = room.GetStaticBoundaryColliders();
        foreach (var rect in boundaryColliderRects)
        {
            var collidable = new StaticBoundaryCollidable(rect, graphicsDevice);
            collisionController.AddCollidable(collidable);
        }
    }

    private void LoadRoomTextures(ContentManager contentManager)
    {
        // Room wall border
        var r = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition);
        r.SetSprite(nameof(Dungeon1Sprite.room_exterior));

        // Floor background
        var f = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.BackgroundPosition);
        f.SetSprite(room.RoomSprite.ToString());

        // Doors
        var doors = room.GetDoors();
        foreach (var door in doors)
        {
            var doorDict = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, door.Position);
            doorDict.SetSprite(door.DoorSprite.ToString());
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach(var trigger in triggers)
        {
            trigger.Update();
        }

        if (projectileManager.ProjectileFired == true)
        {
            projectileManager.executeProjectile();
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

        playerCollision.Update();
    }
}
