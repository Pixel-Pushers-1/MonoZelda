﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies;
using MonoZelda.Player;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Items;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Tiles;

namespace PixelPushers.MonoZelda.Scenes;

internal class DungeonScene : IScene
{
    private GraphicsDevice graphicsDevice;
    private CommandManager commandManager;
    private Player player;

    private EnemyCycler enemyCycler;

    public DungeonScene(GraphicsDevice device, GraphicsDeviceManager gManager, CommandManager cManager) 
    {
        graphicsDevice = device;
        commandManager = cManager;

        player = new Player();

        enemyCycler = new EnemyCycler(commandManager, gManager);
        commandManager.ReplaceCommand(CommandEnum.EnemyCycleCommand, new EnemyCycleCommand(enemyCycler));
    }

    public void LoadContent(ContentManager contentManager)
    {
        // Setup TileDemo
        var tileDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Blocks), SpriteCSVData.Blocks, 0, new Point(300, 300));
        var demoTile = new TileCycleDemo(tileDict, new Point(300, 300));

        //Setup ItemDemo
        var itemDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Items), SpriteCSVData.Items, 0, new Point(450, 100));
        var demoItem = new ItemCycleDemo(itemDict, new Point(450, 100));

        // create the cycle commands
        commandManager.ReplaceCommand(CommandEnum.BlockCycleCommand, new BlockCycleCommand(demoTile));
        commandManager.ReplaceCommand(CommandEnum.ItemCycleCommand, new ItemCycleCommand(demoItem));
        commandManager.ReplaceCommand(CommandEnum.PlayerMoveCommand, new PlayerMoveCommand(player));
        commandManager.ReplaceCommand(CommandEnum.PlayerAttackCommand, new PlayerAttackCommand(player));
        commandManager.ReplaceCommand(CommandEnum.PlayerStandingCommand, new PlayerStandingCommand(player));

        // create spritedict to pass into player controller
        var playerSpriteDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Player), SpriteCSVData.Player, 1, new Point(100, 100));
        player.SetPlayerSpriteDict(playerSpriteDict);

        var enemySpriteDict = new SpriteDict(contentManager.Load<Texture2D>(TextureData.Enemies), SpriteCSVData.Enemies, 1, new Point(100, 100));
        enemyCycler.SetSpriteDicts(enemySpriteDict);
    }

    public void Update(GameTime gameTime)
    {
        enemyCycler.Update(gameTime);
    }

    public void Draw(SpriteBatch batch)
    {
        throw new NotImplementedException();
    }
}
