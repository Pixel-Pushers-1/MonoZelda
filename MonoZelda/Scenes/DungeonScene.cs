﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Link.Projectiles;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Items;

namespace PixelPushers.MonoZelda.Scenes;

internal class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;
    private ProjectileManager projectileManager;
    private PlayerCollision playerCollision;
    private CollidablesManager collidableManager;
    private ItemFactory itemFactory;

    public DungeonScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager gManager, CommandManager cManager, MonoZeldaGame game, CollidablesManager collidableManager) 
    {
        this.graphicsDevice = graphicsDevice;
        commandManager = cManager;
        player = new Player();
        this.collidableManager = collidableManager;

        //create player hitbox
        Collidable playerHitbox = new Collidable(new Rectangle(100, 100, 50, 50), graphicsDevice);
        this.collidableManager.AddCollidableObject(playerHitbox);
        playerCollision = new PlayerCollision(player, playerHitbox, this.collidableManager);

        //create some sample hitboxes
        Collidable itemHitbox1 = new Collidable(new Rectangle(100, 200, 50, 50), graphicsDevice);
        this.collidableManager.AddCollidableObject(itemHitbox1);
        Collidable itemHitbox2 = new Collidable(new Rectangle(200, 200, 100, 100), graphicsDevice);
        this.collidableManager.AddCollidableObject(itemHitbox2);
        Collidable itemHitbox3 = new Collidable(new Rectangle(350, 250, 50, 50), graphicsDevice);
        this.collidableManager.AddCollidableObject(itemHitbox3);        
    }

    public void LoadContent(ContentManager contentManager)
    {
        var _ = new ExperimentalDungeonLoader(contentManager);

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/player"), SpriteCSVData.Projectiles, 0, new Point(0, 0));
        projectileDict.Enabled = false;
        var projectiles = new Projectile(projectileDict, player);
        projectileManager = new ProjectileManager();

        // Temporary itemFactory object to create item according to the room number
        itemFactory = new ItemFactory(graphicsDevice, collidableManager);

        // create a spriteDict for item spawning(Temp - Remove once multi drawing sprite dict is functional)

        // replace required commands
        commandManager.ReplaceCommand(CommandEnum.PlayerMoveCommand, new PlayerMoveCommand(player));
        commandManager.ReplaceCommand(CommandEnum.PlayerAttackCommand, new PlayerAttackCommand(player));
        commandManager.ReplaceCommand(CommandEnum.PlayerStandingCommand, new PlayerStandingCommand(player));
        commandManager.ReplaceCommand(CommandEnum.PlayerUseItemCommand, new PlayerUseItemCommand(projectiles,projectileManager, player));
        commandManager.ReplaceCommand(CommandEnum.PlayerTakeDamageCommand, new PlayerTakeDamageCommand(player));

        // create spritedict to pass into player controller
        var playerSpriteDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Player), SpriteCSVData.Player, 1, new Point(100, 100));
        player.SetPlayerSpriteDict(playerSpriteDict);
    }

    //public spawnItems()
    //{

    //}

    public void Update(GameTime gameTime)
    {
        if(projectileManager.ProjectileFired == true)
        {
            projectileManager.executeProjectile();
        }

        playerCollision.Update();
    }
}
