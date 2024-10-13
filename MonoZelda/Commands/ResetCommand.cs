using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ResetCommand : ICommand
{

    public MonoZeldaGame _game { get; set; }

    public ResetCommand()
    {
    }

    public ResetCommand(MonoZeldaGame game)
    {
        _game = game;
    }

    public void Execute(Keys PressedKey)
    {
        _game?.StartMenu();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
