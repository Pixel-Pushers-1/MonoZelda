using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ExitCommand : ICommand
{
    private MonoZeldaGame game;

    public ExitCommand()
    {
        //empty
    }

    public ExitCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        game.Exit();
    }

    public void UnExecute()
    {
        //empty
    }
}
