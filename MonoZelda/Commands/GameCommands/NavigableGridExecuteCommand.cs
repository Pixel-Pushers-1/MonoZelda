using MonoZelda.Link;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoZelda.UI.NavigableMenus;

namespace MonoZelda.Commands.GameCommands;

public class NavigableGridExecuteCommand : ICommand
{
    private NavigableGrid grid;

    public NavigableGridExecuteCommand() {
        //empty
    }

    public NavigableGridExecuteCommand(NavigableGrid grid) {
        this.grid = grid;
    }

    public void Execute(params object[] metadata) {
        grid?.ExecuteSelection();
    }

    public void UnExecute() {
        //empty
    }
}
