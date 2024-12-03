﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Scenes;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.UI;
using MonoZelda.Save;
using Microsoft.Xna.Framework.Input;


namespace MonoZelda;

public enum GameState
{
    Title,
    Start,
    Reset,
    Quit,
    None
}

public enum GameType {
    Classic,
    Infinite,
}

public class MonoZeldaGame : Game, ISaveable
{
    public static GameTime GameTime { get; private set; }

    private GraphicsDeviceManager graphicsDeviceManager;
    private SpriteBatch spriteBatch;
    private IController controller;
    private CommandManager commandManager;
    private SaveManager saveManager;

    private IScene scene;

    public static int EnemyLevel {get; set;}

    public MonoZeldaGame()
    {
        EnemyLevel = 1;

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // create Command Manager
        commandManager = new CommandManager();

        // create Save Manager
        saveManager = new SaveManager(this);

        // initialize soundManager
        SoundManager.Initialize(Content);

        // Commands that use MonoZeldaGame reference
        commandManager.ReplaceCommand(CommandType.ExitCommand, new ExitCommand(this));
        commandManager.ReplaceCommand(CommandType.StartGameCommand, new StartGameCommand(this));
        commandManager.ReplaceCommand(CommandType.ResetCommand, new ResetCommand(this));
        commandManager.ReplaceCommand(CommandType.PlayerDeathCommand, new PlayerDeathCommand(this));    
    }

    protected override void Initialize()
    {
        // 4x the native game resolution
        graphicsDeviceManager.PreferredBackBufferWidth = 1024;
        graphicsDeviceManager.PreferredBackBufferHeight = 896;
        graphicsDeviceManager.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        TextureData.LoadTextures(Content, GraphicsDevice);

        // Start menu goes first
        StartMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        if (controller is null) {
            // create controller objects
            if (GamePad.GetState(PlayerIndex.One).IsConnected) 
            {
                controller = new GamepadController(commandManager, PlayerIndex.One);

            }
            else 
            {
                controller = new KeyboardController(commandManager);
            }
        }
        if (PlayerState.IsDead)
        {
            commandManager.Execute(CommandType.PlayerDeathCommand);
            PlayerState.IsDead = false;
            PlayerState.Initialize();

        }

        GameTime = gameTime;
        controller.Update(gameTime);
        scene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        SamplerState samplerState = new();
        samplerState.Filter = TextureFilter.Point;
        spriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState);

        // SpriteDrawer draws all drawables
        SpriteDrawer.Draw(spriteBatch, gameTime);
        scene.Draw(spriteBatch);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    protected void LoadScene(IScene scene)
    {
        // Clean state to start a new scene
        SpriteDrawer.Reset();
        this.scene = scene;
        scene.LoadContent(Content);
    }

    public void StartMenu()
    {
        LoadScene(new MainMenuScene(commandManager));
    }

    public void StartDungeon()
    {
        // Preventing the StartCommand from activating when it shouldn't. -js
        if (scene is MainMenuScene)
        {
            SoundManager.StopSound("LOZ_Intro");
            LoadDungeon("Room1");
        }
    }

    public void LoadDungeon(string roomName)
    {
        LoadScene(new SceneManager(roomName, GraphicsDevice, commandManager));
    }

    public void ResetGame()
    {
        SoundManager.ClearSoundDictionary();
        HUDMapWidget.Reset();
        InventoryMapWidget.Reset();
        LoadScene(new MainMenuScene(commandManager));
        PlayerState.Initialize();
        LoadScene(new MainMenuScene(commandManager));
    }

    public void Save(SaveState save)
    {
        if(scene is SceneManager sceneManager)
        {
            sceneManager.Save(save);
        }
    }

    public void Load(SaveState save)
    {
        SoundManager.ClearSoundDictionary();
        HUDMapWidget.Reset();

        var loadDungeon = new SceneManager(save.RoomName, GraphicsDevice, commandManager);
        loadDungeon.Load(save);

        LoadScene(loadDungeon);
    }
}
