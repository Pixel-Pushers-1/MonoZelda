using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Controllers;
using System;

namespace PixelPushers.MonoZelda.Commands;

public class StartGameCommand : ICommand
{
    private IController controller;
    private MonoZeldaGame game;

    public StartGameCommand()
    {

    }

    public StartGameCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public GameState Execute(Keys PressedKey)
    {
        game?.StartDungeon();

        return GameState.Start;
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }
}
