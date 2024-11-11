namespace MonoZelda.Commands.GameCommands;

public class EndGameCommand : ICommand
{
    private MonoZeldaGame game;

    public EndGameCommand()
    {
        //empty
    }

    public EndGameCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        game?.EndGame();
    }

    public void UnExecute()
    {
        //empty
    }
}
