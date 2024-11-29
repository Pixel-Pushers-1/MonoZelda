using MonoZelda.Save;
using MonoZelda.Sound;

namespace MonoZelda.Commands.GameCommands;

public class QuickSaveCommand : ICommand
{
    private SaveManager saveManager;

    public QuickSaveCommand()
    {
        //empty
    }

    public QuickSaveCommand(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    public void Execute(params object[] metadata)
    {
        saveManager?.Save();
    }

    public void UnExecute()
    {
        //empty
    }
}

