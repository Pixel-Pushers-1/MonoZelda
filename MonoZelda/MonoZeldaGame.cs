using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Scenes;
using MonoZelda.Collision;
using MonoZelda.Commands;

namespace PixelPushers.MonoZelda;

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
    private GraphicsDeviceManager graphicsDeviceManager;
    private SpriteBatch spriteBatch;
    private KeyboardController keyboardController;
    private MouseController mouseController;
    private CommandManager commandManager;
    private CollidablesManager collidableManager;
    private ExperimentalDungeonLoader dungeonLoader;

    private IScene scene;

    public MonoZeldaGame()
    {
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        commandManager = new CommandManager();

        // Commands that use MonoZeldaGame reference
        commandManager.ReplaceCommand(CommandEnum.ExitCommand, new ExitCommand(this));
        commandManager.ReplaceCommand(CommandEnum.StartGameCommand, new StartGameCommand(this));
        commandManager.ReplaceCommand(CommandEnum.ResetCommand, new ResetCommand(this));

        keyboardController = new KeyboardController(commandManager);
        mouseController = new MouseController(commandManager);

        collidableManager = new();
    }

    protected override void Initialize()
    {
        // 4x the native game resolution
        graphicsDeviceManager.PreferredBackBufferWidth = 1024;
        graphicsDeviceManager.PreferredBackBufferHeight = 896;
        graphicsDeviceManager.ApplyChanges();

        dungeonLoader = new(Content, collidableManager, graphicsDeviceManager.GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Start menu goes first
        StartMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardController.Update();
        mouseController.Update();
        scene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();

        // SpriteDrawer draws all drawables
        SpriteDrawer.Draw(spriteBatch, gameTime);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    protected void LoadScene(IScene scene)
    {
        // Clean state to start a new scene
        SpriteDrawer.Reset();
        collidableManager.Clear();
        this.scene = scene;
        scene.LoadContent(Content);
    }

    public void StartMenu()
    {
        LoadScene(new MainMenu(GraphicsDevice));
    }

    public void StartDungeon()
    {
        // Preventing the StartCommand from activating when it shouldn't. -js
        if (scene is MainMenu)
        {
            // TODO: Passing MonoZeldaGame smells. It's used by some things to LoadContent, SpriteDict multiple AddSprite()
            LoadDungeon("Room1");
        }
    }

    public void LoadDungeon(string roomName)
    {
        // TODO: Some of these overlaods will go away when we get instanced SpriteDicts. -js
        LoadScene(new DungeonScene(roomName, dungeonLoader, GraphicsDevice, graphicsDeviceManager, commandManager, this, collidableManager));
    }
}
