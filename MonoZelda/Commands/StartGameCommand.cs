namespace MonoZelda.Commands;

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

    public void Execute(params object[] metadata)
    {
        game?.StartDungeon();
    }

    public void UnExecute()
    {
        //empty
    }
}
