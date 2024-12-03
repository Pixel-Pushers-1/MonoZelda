using MonoZelda.Link;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoZelda.UI.NavigableMenus;

namespace MonoZelda.Commands.GameCommands;

public class NavigableGridExecuteCommand : ICommand
{
    private INavigableGrid grid;

    public NavigableGridExecuteCommand() {
        //empty
    }

    public NavigableGridExecuteCommand(INavigableGrid grid) {
        this.grid = grid;
    }

    public void Execute(params object[] metadata) {
        grid?.ExecuteSelection();
    }

    public void UnExecute() {
        //empty
    }
}
