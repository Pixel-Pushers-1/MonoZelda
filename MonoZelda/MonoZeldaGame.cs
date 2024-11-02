using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Scenes;
using MonoZelda.Sound;

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
    private CollisionController collisionController;
    private CommandManager commandManager;
    private IDungeonRoomLoader dungeonManager;
    private IScene scene;

    public MonoZeldaGame()
    {
        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // create Command Manager
        commandManager = new CommandManager();

        // Commands that use MonoZeldaGame reference
        commandManager.ReplaceCommand(CommandType.ExitCommand, new ExitCommand(this));
        commandManager.ReplaceCommand(CommandType.StartGameCommand, new StartGameCommand(this));
        commandManager.ReplaceCommand(CommandType.ResetCommand, new ResetCommand(this));

        // create controller objects
        keyboardController = new KeyboardController(commandManager);
        mouseController = new MouseController(commandManager);
        collisionController = new CollisionController(commandManager);
    }

    protected override void Initialize()
    {
        // 4x the native game resolution
        graphicsDeviceManager.PreferredBackBufferWidth = 1024;
        graphicsDeviceManager.PreferredBackBufferHeight = 896;
        graphicsDeviceManager.ApplyChanges();

        dungeonManager = new DungeonManager();
        SoundManager.Initialize(Content);

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
        GameTime = gameTime;
        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);
        scene.Update(gameTime);
        collisionController.Update(gameTime);

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
        collisionController.Clear();
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
            SoundManager.StopSound("LOZ_Intro");
            LoadDungeon("Room0");
        }
    }

    public void LoadDungeon(string roomName)
    {
        var room = dungeonManager.LoadRoom(roomName);
        commandManager.ReplaceCommand(CommandType.LoadRoomCommand, new LoadRoomCommand(this, room));

        LoadScene(new DungeonScene(GraphicsDevice, commandManager, collisionController, room));
    }

    public CollisionController GetCollisionController() 
    {
        return collisionController;
    }
}
