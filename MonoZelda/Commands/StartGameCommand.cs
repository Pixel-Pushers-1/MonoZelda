using Microsoft.Xna.Framework.Input;
using System;

namespace PixelPushers.MonoZelda.Commands;

public class StartGameCommand : ICommand
{
    public MonoZeldaGame _game { get; set; }

    public StartGameCommand()
    {

    }

    public StartGameCommand(MonoZeldaGame game)
    {
        _game = game;
    }

    public void Execute(Keys PressedKey)
    {
        _game?.StartDungeon();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
