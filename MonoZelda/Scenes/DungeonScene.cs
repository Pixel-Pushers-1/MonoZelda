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
using MonoZelda.Commands.CollisionCommands;
using MonoZelda.Trigger;

namespace MonoZelda.Scenes;

public class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;
    private ProjectileManager projectileManager;
    private PlayerCollision playerCollision;
    private CollisionController collisionController;
    private List<ITrigger> triggers;
    private ItemFactory itemFactory;
    private EnemyFactory enemyFactory;
    private List<IEnemy> enemies = new();
    private Dictionary<IEnemy, EnemyCollision> enemyDictionary = new();
    private List<EnemyCollision> enemyCollisions = new();
    private List<EnemyProjectileCollision> enemyProjectileCollisions = new();
    private IDungeonRoom room;
    private string roomName;


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

        //create player and player collision
        player = new Player();
        Collidable playerHitbox = new Collidable(new Rectangle(100, 100, 50, 50), graphicsDevice, CollidableType.Player);
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollision(player, playerHitbox, collisionController);

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/player"), SpriteCSVData.Projectiles, 0, new Point(0, 0));
        projectileManager = new ProjectileManager(collisionController, graphicsDevice, projectileDict);

        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerSetProjectileCommand, new PlayerSetProjectileCommand(projectileManager, player));   
        commandManager.ReplaceCommand(CommandType.PlayerFireSwordBeamCommand, new PlayerFireSwordBeamCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerFireProjectileCommand, new PlayerFireProjectileCommand(projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerStaticCollisionCommand, new PlayerStaticCollisionCommand(playerCollision));
        commandManager.ReplaceCommand(CommandType.PlayerEnemyCollisionCommand, new PlayerEnemyCollisionCommand(playerCollision));
        commandManager.ReplaceCommand(CommandType.PlayerEnemyProjectileCollisionCommand, new PlayerEnemyProjectileCollisionCommand(player));

        // create spritedict to pass into player controller
        var playerSpriteDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Player), SpriteCSVData.Player, 1, new Point(100, 100));
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
            enemyDictionary.Add(enemy, new EnemyCollision(enemy, collisionController, enemy.Width, enemy.Height));
        }
    }

    private void CreateStaticColliders()
    {
        var colliderRects = room.GetStaticColliders();
        foreach (var rect in colliderRects)
        {
            var collidable = new Collidable(rect, graphicsDevice, CollidableType.Static);
            collisionController.AddCollidable(collidable);
        }
    }

    private void LoadRoomTextures(ContentManager contentManager)
    {
        var dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);

        // Room wall border
        var r = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition);
        r.SetSprite(nameof(Dungeon1Sprite.room_exterior));

        // Floor background
        var f = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Background, DungeonConstants.BackgroundPosition);
        f.SetSprite(room.RoomSprite.ToString());

        // Doors
        var doors = room.GetDoors();
        foreach (var door in doors)
        {
            var doorDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Background, door.Position);
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

        foreach(KeyValuePair<IEnemy, EnemyCollision> entry in enemyDictionary)
        {
            entry.Key.Update(gameTime);
            entry.Value.Update(entry.Key.Width, entry.Key.Height);
        }

        playerCollision.Update();
    }
}
