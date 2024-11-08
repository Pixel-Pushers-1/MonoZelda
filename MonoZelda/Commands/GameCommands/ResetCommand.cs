namespace MonoZelda.Commands.GameCommands;

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

    public void Execute(params object[] metadata)
    {
        game?.ResetGame();
    }

    public void UnExecute()
    {
        //empty
    }
}
