using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Commands;
using MonoZelda.Sprites;
using System.Collections.Generic;
using MonoZelda.Link;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sound;

namespace MonoZelda.Scenes;

public class LinkDeathScene : Scene
{
    // constants
    private static readonly Point RED_BACKGROUND_SIZE = new Point(192*4, 112*4);
    private const string GameOverString = "Game Over";
    private const float DIRECTION_CHANGE_TIME = 0.1f;
    private const float ROTATING_TIME = 3f;
    private const float KILL_LINK_TIME = 4f;
    private const float GAME_OVER_DISPLAY_TIME = 7f;

    // variables
    private float directionChangeTimer;
    private float sceneTimer;
    private bool displayString;
    private Point Center;
    private Direction FakeLinkDirection;
    private ICommand resetGameCommand;
    private SpriteFont spriteFont;
    private BlankSprite GameOverScreen;
    private BlankSprite RedBackground;
    private BlankSprite BlackBackground;
    private GraphicsDevice graphicsDevice;
    private SpriteDict FakeLink;

    private readonly Dictionary<Direction, string> DirectionStringMap = new()
    {
        {Direction.Up,"up"},
        {Direction.Down,"down"},
        {Direction.Left,"left"},
        {Direction.Right,"right"},
    };

    private readonly Dictionary<Direction, Direction> ChangeDirectionMap = new()
    {
        {Direction.Up,Direction.Right},
        {Direction.Down,Direction.Left},
        {Direction.Left,Direction.Up},
        {Direction.Right,Direction.Down},
    };

    public LinkDeathScene(ICommand resetGameCommand, GraphicsDevice graphicsDevice)
    {
        displayString = false;
        directionChangeTimer = 0;
        FakeLinkDirection = Direction.Down;
        this.graphicsDevice = graphicsDevice;
        this.resetGameCommand = resetGameCommand;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Load SprintFont and calculate center for game over text
        spriteFont ??= contentManager.Load<SpriteFont>("Fonts/Basic");
        Center = new Point((graphicsDevice.Viewport.Width / 2)  - 128, (graphicsDevice.Viewport.Height / 2) + 16);

        // create FakeLink
        FakeLink = new SpriteDict(SpriteType.Player, SpriteLayer.Triforce, PlayerState.Position);
        FakeLink.SetSprite($"standing_{DirectionStringMap[FakeLinkDirection]}");

        // create black and red backgrounds
        RedBackground = new BlankSprite(SpriteLayer.Triforce - 1, DungeonConstants.BackgroundPosition, RED_BACKGROUND_SIZE, Color.Red);
        BlackBackground = new BlankSprite(SpriteLayer.HUD + 1, DungeonConstants.DungeonPosition, new Point(graphicsDevice.Viewport.Width,graphicsDevice.Viewport.Height), Color.Black);
        BlackBackground.Enabled = false;

        // play LinkDeath Sound
        SoundManager.ClearSoundDictionary();
        SoundManager.PlaySound("LOZ_Link_Die", false);
    }

    public override void Draw(SpriteBatch batch)
    {
        if (displayString == true)
        {
            batch.DrawString(spriteFont, GameOverString, Center.ToVector2(), Color.White);
        }
}

    public override void Update(GameTime gameTime)
    {
        sceneTimer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;

        if (sceneTimer < ROTATING_TIME)
        {
            directionChangeTimer += (float)MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            if (directionChangeTimer > DIRECTION_CHANGE_TIME)
            {
                FakeLinkDirection = ChangeDirectionMap[FakeLinkDirection];
                FakeLink.SetSprite($"standing_{DirectionStringMap[FakeLinkDirection]}");
                directionChangeTimer = 0;
            }
        }
        else if(sceneTimer < KILL_LINK_TIME)
        {
            FakeLink.SetSprite("cloud");
        }
        else if(sceneTimer < GAME_OVER_DISPLAY_TIME)
        {
            BlackBackground.Enabled = true;
            RedBackground.Unregister();
            FakeLink.Unregister();
            displayString = true;
        }
        else
        {
            BlackBackground.Unregister();
            resetGameCommand.Execute();
        }
    }
}
