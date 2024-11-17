using MonoZelda.Save;

namespace MonoZelda.Commands.GameCommands;

public class QuickLoadCommand : ICommand
{
    private SaveManager saveManager;

    public QuickLoadCommand()
    {
        //empty
    }

    public QuickLoadCommand(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    public void Execute(params object[] metadata)
    {
        saveManager?.Load();
    }

    public void UnExecute()
    {
        //empty
    }
}

