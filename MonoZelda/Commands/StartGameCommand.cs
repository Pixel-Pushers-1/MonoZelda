using Microsoft.Xna.Framework.Input;
using System;

namespace PixelPushers.MonoZelda.Commands;

public class StartGameCommand : ICommand
{
    public MonoZeldaGame Game { get; set; }

    public StartGameCommand()
    {

    }

    public StartGameCommand(MonoZeldaGame game)
    {
        Game = game;
    }

    public void Execute(Keys PressedKey)
    {
        Game?.StartDungeon();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
