using MonoZelda.Sound;

namespace MonoZelda.Commands.GameCommands;

public class MuteCommand : ICommand
{
    private MonoZeldaGame game;

    public MuteCommand()
    {
        //empty
    }

    public MuteCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        SoundManager.ChangeMuteState();
    }

    public void UnExecute()
    {
        //empty
    }
}

