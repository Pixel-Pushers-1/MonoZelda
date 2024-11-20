namespace MonoZelda.Commands.GameCommands;

public class EmptyCommand : ICommand
{
    public void Execute(params object[] metadata)
    {
        //empty
    }

    public void UnExecute() {
        //empty
    }
}
