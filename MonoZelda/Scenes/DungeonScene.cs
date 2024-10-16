using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Link.Projectiles;
using MonoZelda.Link;
using MonoZelda.Collision;
using PixelPushers.MonoZelda.Controllers;
using System;
using MonoZelda.Dungeons;
using MonoZelda.Commands;
using MonoZelda.Scenes;
using MonoZelda.Sprites;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace PixelPushers.MonoZelda.Scenes;

public class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;
    private ProjectileManager projectileManager;
    private IDungeonRoom room;

    private PlayerCollision playerCollision;
    private CollisionController collisionController;


    public DungeonScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room) 
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
        this.room = room;
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
        projectileDict.Enabled = false;
        var projectiles = new Projectile(projectileDict, player);
        projectileManager = new ProjectileManager();

        // replace required commands
        commandManager.ReplaceCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand(player));
        commandManager.ReplaceCommand(CommandType.PlayerUseItemCommand, new PlayerUseItemCommand(projectiles,projectileManager, player));
        commandManager.ReplaceCommand(CommandType.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(player));

        // create spritedict to pass into player controller
        var playerSpriteDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Player), SpriteCSVData.Player, 1, new Point(100, 100));
        player.SetPlayerSpriteDict(playerSpriteDict);

        //create some sample hitboxes
        Collidable itemHitbox1 = new Collidable(new Rectangle(100, 200, 50, 50), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(itemHitbox1);
        Collidable itemHitbox2 = new Collidable(new Rectangle(200, 200, 100, 100), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(itemHitbox2);
        Collidable itemHitbox3 = new Collidable(new Rectangle(350, 250, 50, 50), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(itemHitbox3);
    }

    private void LoadRoom(ContentManager contentManager)
    {
        LoadRoomTextures(contentManager);
        CreateStaticColliders();
        SpawnItems();
        SpawnEnemies();
    }

    private void SpawnItems()
    {
        foreach(var itemSpwan in room.GetItemSpawns())
        {
            // TODO: Spawn the item
        }
    }

    private void SpawnEnemies()
    {
        foreach(var enemySpawn in room.GetEnemySpawns())
        {
            // TODO: Spawn the enemy
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
        if(projectileManager.ProjectileFired == true)
        {
            projectileManager.executeProjectile();
        }
        // not doing anything currently
        playerCollision.Update();
    }
}
