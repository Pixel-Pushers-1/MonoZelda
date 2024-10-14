using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ResetCommand : ICommand
{

    public MonoZeldaGame Game { get; set; }

    public ResetCommand()
    {
    }

    public ResetCommand(MonoZeldaGame game)
    {
        Game = game;
    }

    public void Execute(Keys PressedKey)
    {
        Game?.GetCollisionController().Reset();
        Game?.StartMenu();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
