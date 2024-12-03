using System.Diagnostics;

namespace MonoZelda.Commands.GameCommands;

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
        GameType gameType = (GameType) metadata[0];
        game?.StartDungeon();
    }

    public void UnExecute()
    {
        //empty
    }
}
