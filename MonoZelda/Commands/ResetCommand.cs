using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class ResetCommand : ICommand
{
    private MonoZeldaGame game;

    public ResetCommand()
    {
        //empty
    }

    public ResetCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(Keys PressedKey)
    {
        game?.GetCollisionController().Reset();
        game?.StartMenu();
    }

    public void UnExecute()
    {
        //empty
    }
}
