using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Scenes;

namespace PixelPushers.MonoZelda;

public class MonoZeldaGame : Game
{
    private GraphicsDeviceManager graphicsDeviceManager;
    private SpriteBatch spriteBatch;
    private KeyboardController keyboardController;
    private MouseController mouseController;
    private CollisionController collisionController;
    private CommandManager commandManager;

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
        collisionController = new CollisionController(commandManager);
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
        
        // Start menu goes first
        StartMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        keyboardController.Update(gameTime);
        mouseController.Update(gameTime);
        collisionController.Update(gameTime);
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
            LoadScene(new DungeonScene(GraphicsDevice, graphicsDeviceManager, commandManager, this, collisionController));
        }
    }
}
