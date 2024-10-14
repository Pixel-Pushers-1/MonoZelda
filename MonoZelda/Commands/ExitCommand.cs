using System;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ExitCommand : ICommand
{
    public MonoZeldaGame Game { get; set; }

    public ExitCommand()
    {
    }

    public ExitCommand(MonoZeldaGame game)
    {
        Game = game;
    }

    public void Execute(Keys PressedKey)
    {
        Game.Exit();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
