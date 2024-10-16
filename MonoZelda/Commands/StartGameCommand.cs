using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class StartGameCommand : ICommand
{
    private MonoZeldaGame game;

    public StartGameCommand()
    { 
        //empty
    }

    public StartGameCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(Keys PressedKey)
    {
        game?.StartDungeon();
    }

    public void UnExecute()
    {
        //empty
    }
}
