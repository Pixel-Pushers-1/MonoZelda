using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands;

internal class ToggleInventoryCommand : ICommand
{
    private readonly DungeonScene dungeonScene;
    
    public ToggleInventoryCommand()
    {
    }
    
    public ToggleInventoryCommand(DungeonScene dungeonScene)
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