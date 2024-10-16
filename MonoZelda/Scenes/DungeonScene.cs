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

namespace PixelPushers.MonoZelda.Scenes;

public class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;
    private ProjectileManager projectileManager;

    private PlayerCollision playerCollision;
    private CollisionController collisionController;

    public DungeonScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController) 
    {
        this.graphicsDevice = graphicsDevice;
        this.commandManager = commandManager;
        this.collisionController = collisionController;
    }

    public void LoadContent(ContentManager contentManager)
    {
        var _ = new ExperimentalDungeonLoader(contentManager);

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
