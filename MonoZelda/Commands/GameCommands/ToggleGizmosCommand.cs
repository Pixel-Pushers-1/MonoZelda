using MonoZelda.Sprites;
using System.Diagnostics;

namespace MonoZelda.Commands.GameCommands;

public class ToggleGizmosCommand : ICommand
{
    public ToggleGizmosCommand()
    {
        //empty
    }

    public void Execute(params object[] metadata)
    {
        SpriteDrawer.DrawGizmos = !SpriteDrawer.DrawGizmos;
    }

    public void UnExecute()
    {
        //empty
    }
}
