using System;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ExitCommand : ICommand
{
    public MonoZeldaGame _game { get; set; }

    public ExitCommand()
    {
    }

    public ExitCommand(MonoZeldaGame game)
    {
        _game = game;
    }

    public void Execute(Keys PressedKey)
    {
        _game.Exit();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
