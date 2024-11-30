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
using Microsoft.Xna.Framework.Content;
using System;


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

    public static Effect effect;

    private IScene scene;

    private float flickerTime;

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

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        TextureData.LoadTextures(Content, GraphicsDevice);
        effect = Content.Load<Effect>("Shaders/LitSprite");

        var numLineSegmentsParameter = effect.Parameters["num_line_segments"];
        if(numLineSegmentsParameter != null)
        {
            numLineSegmentsParameter.SetValue(0);
        }

        var lineSegmentsParameter = effect.Parameters["line_segments"];
        if(lineSegmentsParameter != null)
        {
            var testSegments = new Vector4[0];
            lineSegmentsParameter.SetValue(testSegments);
        }

        var menuPositionParameter = effect.Parameters["menu_position"];
        if(menuPositionParameter != null)
        {
            menuPositionParameter.SetValue(GraphicsDevice.Viewport.Height - 192);
        }

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
        flickerTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        Matrix view = Matrix.Identity;

        int width = GraphicsDevice.Viewport.Width;
        int height = GraphicsDevice.Viewport.Height;
        Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);

        var VP = view * projection;

        effect.Parameters["view_projection"].SetValue(VP);
        var player_position = new Vector2(PlayerState.Position.X, height - PlayerState.Position.Y);
        var posParameter = effect.Parameters["player_position"];
        if(posParameter != null)
            posParameter.SetValue(player_position);    

        
        var lightPositionsParameter = effect.Parameters["light_positions"];
        if(lightPositionsParameter != null)
        {
            var testLights = new Vector3[4]
            {
                new Vector3(256, 64, 0),
                new Vector3(768, 64, 0),
                new Vector3(256, 640, 0),
                new Vector3(768, 640, 0)
            };

            lightPositionsParameter.SetValue(testLights);
        }

        var lightColorsParameter = effect.Parameters["light_colors"];
        if(lightColorsParameter != null)
        {
            var testColors = new Vector3[4];
            for (int i = 0; i < testColors.Length; i++)
            {
                float intensity = 0.65f + 0.15f * (float)Math.Sin(flickerTime * 4.0f + i);
                testColors[i] = new Vector3(1, 1, intensity);
            }

            lightColorsParameter.SetValue(testColors);
        }

        var lightActiveParameter = effect.Parameters["light_active"];
        if(lightActiveParameter != null)
        {
            var testActive = new float[4] { 0, 0, 0, 0 };
            lightActiveParameter.SetValue(testActive);
        }

        var numLightsParameter = effect.Parameters["num_lights"];
        if(numLightsParameter != null)
        {
            numLightsParameter.SetValue(4);
        }

        effect.Techniques[0].Passes[0].Apply();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, null, null, effect);

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
        LoadScene(new MainMenuScene(GraphicsDevice));
    }
}
