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

    public void Execute(Keys PressedKey)
    {
        game.Exit();
    }

    public void UnExecute()
    {
        //empty
    }
}
