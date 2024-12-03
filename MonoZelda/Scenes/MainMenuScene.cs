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
    private CommandManager commandManager;
    private UIGrid uiGrid;
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
        var dict = new SpriteDict(SpriteType.Title, 0, new Point(0,0));
        dict.SetSprite("title");

        //set up ui grid
        SpriteDict classicSelectedButton = new SpriteDict(SpriteType.Title, 2, classicButtonPosition);
        classicSelectedButton.SetSprite("button_classic_selected");
        SpriteDict infiniteSelectedButton = new SpriteDict(SpriteType.Title, 2, infiniteButtonPosition);
        infiniteSelectedButton.SetSprite("button_infinite_selected");

        UIGridItem[,] tempGrid = new UIGridItem[1, 2];
        tempGrid[0, 0] = new UIGridItem(classicSelectedButton, commandManager, CommandType.StartGameCommand, GameType.Classic);
        tempGrid[0, 1] = new UIGridItem(infiniteSelectedButton, commandManager, CommandType.StartGameCommand, GameType.Infinite);
        uiGrid = new UIGrid(tempGrid);

        //replace commands
        commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new NavigableGridMoveCommand(uiGrid));
        commandManager.ReplaceCommand(CommandType.NavigableGridExecuteCommand, new NavigableGridExecuteCommand(uiGrid));

        SoundManager.PlaySound("LOZ_Intro", true);
    }
}