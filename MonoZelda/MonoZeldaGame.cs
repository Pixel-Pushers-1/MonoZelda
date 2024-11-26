using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Scenes;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.UI;
using System.Diagnostics;


namespace MonoZelda;

public enum GameState
{
    Title,
    Start,
    Reset,
    Quit,
    None
}

public class MonoZeldaGame : Game
{
    public static GameTime GameTime { get; private set; }

    private GraphicsDeviceManager graphicsDeviceManager;
    private SpriteBatch spriteBatch;
    private KeyboardController keyboardController;
    private MouseController mouseController;
    private CommandManager commandManager;

    private IScene scene;

    public MonoZeldaGame()
    {
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // create Command Manager
        commandManager = new CommandManager();

        // initialize soundManager
        SoundManager.Initialize(Content);

        // Commands that use MonoZeldaGame reference
        commandManager.ReplaceCommand(CommandType.ExitCommand, new ExitCommand(this));
        commandManager.ReplaceCommand(CommandType.StartGameCommand, new StartGameCommand(this));
        commandManager.ReplaceCommand(CommandType.ResetCommand, new ResetCommand(this));
        commandManager.ReplaceCommand(CommandType.PlayerDeathCommand, new PlayerDeathCommand(this));    

        // create controller objects
        keyboardController = new KeyboardController(commandManager);
        mouseController = new MouseController(commandManager);
    }

    protected override void Initialize()
    {
        // 4x the native game resolution
        graphicsDeviceManager.PreferredBackBufferWidth = 1024;
        graphicsDeviceManager.PreferredBackBufferHeight = 896;
        graphicsDeviceManager.ApplyChanges();

        base.Initialize();
    }

    SpriteFont testFont;

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        TextureData.LoadTextures(Content, GraphicsDevice);

        // Start menu goes first
        StartMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        if (PlayerState.IsDead)
        {
            commandManager.Execute(CommandType.PlayerDeathCommand);
            PlayerState.IsDead = false;
            PlayerState.Initialize();

        }

        GameTime = gameTime;
        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);
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
        LoadScene(new MainMenuScene(GraphicsDevice));
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
        LoadScene(new DungeonScene(roomName, GraphicsDevice, commandManager));
    }

    public void ResetGame()
    {
        SoundManager.ClearSoundDictionary();
        HUDMapWidget.Reset();
        InventoryMapWidget.Reset();
        PlayerState.Initialize();
        LoadScene(new MainMenuScene(GraphicsDevice));
    }
}
