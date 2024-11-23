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
    private NavigableGrid uiGrid;

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
        NavigableGridItem[,] tempGrid = new NavigableGridItem[3, 2];
        tempGrid[0, 0] = new NavigableGridItem(commandManager, CommandType.StartGameCommand, GameType.classic);
        tempGrid[1, 0] = tempGrid[0, 0];
        tempGrid[2, 0] = tempGrid[0, 0];
        tempGrid[0, 1] = new NavigableGridItem(commandManager, CommandType.StartGameCommand, GameType.infiniteEasy);
        tempGrid[1, 1] = new NavigableGridItem(commandManager, CommandType.StartGameCommand, GameType.infiniteMedium);
        tempGrid[2, 1] = new NavigableGridItem(commandManager, CommandType.StartGameCommand, GameType.infiniteHard);
        uiGrid = new NavigableGrid(tempGrid);

        //replace commands
        commandManager.ReplaceCommand(CommandType.NavigableGridMoveCommand, new NavigableGridMoveCommand(uiGrid));
        commandManager.ReplaceCommand(CommandType.NavigableGridExecuteCommand, new NavigableGridExecuteCommand(uiGrid));

        SoundManager.PlaySound("LOZ_Intro", true);
    }
}