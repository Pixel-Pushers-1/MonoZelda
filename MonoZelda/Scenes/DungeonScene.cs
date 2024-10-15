using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Link.Projectiles;
using MonoZelda.Scenes;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Items;
using PixelPushers.MonoZelda.Items.ItemClasses;

namespace PixelPushers.MonoZelda.Scenes;

internal class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;
    private ProjectileManager projectileManager;
    private IDungeonRoomLoader dungeonLoader;
    private MonoZeldaGame game;

    private PlayerCollision playerCollision;
    private CollisionController collisionController;
    private ItemFactory itemFactory;
    private string roomName;

    public DungeonScene(string roomName, IDungeonRoomLoader dungeonLoader, GraphicsDevice graphicsDevice, GraphicsDeviceManager gManager, CommandManager cManager, MonoZeldaGame game, CollisionController collisionController) 
    {
        this.graphicsDevice = graphicsDevice;
        this.dungeonLoader = dungeonLoader;
        this.roomName = roomName;
        this.game = game;
        this.collisionController = collisionController;
        commandManager = cManager;
        player = new Player();
    }

    public void LoadContent(ContentManager contentManager)
    {
        // TODO: This belongs in the Scene that Loads room scenes.
        var room = dungeonLoader.LoadRoom(roomName);
        commandManager.ReplaceCommand(CommandEnum.LoadRoomCommand, new LoadRoomCommand(game, room));
        // TODO: Make Rooms a subscene... Decorator pattern? hopefully not -js

        // create projectile object and spriteDict
        var projectileDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/player"), SpriteCSVData.Projectiles, 0, new Point(0, 0));
        projectileDict.Enabled = false;
        var projectiles = new Projectile(projectileDict, player);
        projectileManager = new ProjectileManager();

        // Creating player collidable
        Collidable playerHitbox = new Collidable(new Rectangle(100, 100, 50, 50), graphicsDevice, "Player");
        collisionController.AddCollidable(playerHitbox);
        playerCollision = new PlayerCollision(player, playerHitbox, this.collisionController);

        // Temporary itemFactory object to create item according to the room number
        itemFactory = new ItemFactory(graphicsDevice, collisionController);

        //spawn some temporary items for item hitbox testing
        var compassDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, new Point(0, 0));
        IItem Compass = itemFactory.CreateItem<Compass>();
        Compass.itemSpawn(compassDict, new Point(128, 448));
        var keyDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, new Point(0, 0));
        IItem Key = itemFactory.CreateItem<Key>();
        Key.itemSpawn(keyDict, new Point(448, 448));

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

    public void Update(GameTime gameTime)
    {
        if(projectileManager.ProjectileFired == true)
        {
            projectileManager.executeProjectile();
        }

        playerCollision.Update();
    }
}
