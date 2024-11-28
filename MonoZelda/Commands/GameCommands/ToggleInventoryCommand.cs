using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands;

internal class ToggleInventoryCommand : ICommand
{
    private readonly SceneManager dungeonScene;
    
    public ToggleInventoryCommand()
    {
    }
    
    public ToggleInventoryCommand(SceneManager dungeonScene)
    {
        this.dungeonScene = dungeonScene;
    }
    
    public void Execute(params object[] metadata)
    {
        dungeonScene?.ToggleInventory();
    }

    public void UnExecute()
    {
    }
}