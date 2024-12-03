using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using MonoZelda.UI.NavigableMenus;

namespace MonoZelda.Scenes;

public class MainMenuScene : Scene
{
    private static readonly Point classicButtonPosition = new Point(1024 - 16, 896 - 16 - 21 * 4 - 16);
    private static readonly Point infiniteButtonPosition = new Point(1024 - 16, 896 - 16);

    private CommandManager commandManager;
    private NavigableGrid uiGrid;
    private SpriteDict background;
    private SpriteDict classicButton;
    private SpriteDict infiniteButton;

    public MainMenuScene(CommandManager commandManager)
    {
        this.commandManager = commandManager;
    }

    public override void Update(GameTime gameTime)
    {
        // To-do: animate waterfall
    }

    public override void LoadContent(ContentManager contentManager)
    {
        background = new SpriteDict(SpriteType.Title, 0, new Point(0,0));
        background.SetSprite("title");
        classicButton = new SpriteDict(SpriteType.Title, 1, classicButtonPosition);
        classicButton.SetSprite("button_classic_unselected");
        infiniteButton = new SpriteDict(SpriteType.Title, 1, infiniteButtonPosition);
        infiniteButton.SetSprite("button_infinite_unselected");

        //set up ui grid
        SpriteDict classicSelectedButton = new SpriteDict(SpriteType.Title, 2, classicButtonPosition);
        classicSelectedButton.SetSprite("button_classic_selected");
        SpriteDict infiniteSelectedButton = new SpriteDict(SpriteType.Title, 2, infiniteButtonPosition);
        infiniteSelectedButton.SetSprite("button_infinite_selected");

        NavigableGridItem[,] tempGrid = new NavigableGridItem[1, 2];
        tempGrid[0, 0] = new NavigableGridItem(classicSelectedButton, commandManager, CommandType.StartGameCommand, GameType.Classic);
        tempGrid[0, 1] = new NavigableGridItem(infiniteSelectedButton, commandManager, CommandType.StartGameCommand, GameType.Infinite);
        uiGrid = new NavigableGrid(tempGrid);

        //replace commands
        commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new NavigableGridMoveCommand(uiGrid));
        commandManager.ReplaceCommand(CommandType.NavigableGridExecuteCommand, new NavigableGridExecuteCommand(uiGrid));

        SoundManager.PlaySound("LOZ_Intro", true);
    }
}