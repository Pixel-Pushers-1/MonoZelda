using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerCycleEquippableCommand : ICommand
{
    private EquippableManager equippableManager;
    public PlayerCycleEquippableCommand()
    {
        //empty
    }

    public PlayerCycleEquippableCommand(EquippableManager equippableManager)
    {
        this.equippableManager = equippableManager;
    }

    public void Execute(params object[] metadata)
    {
        equippableManager?.CycleEquippedUtility();
    }

    public void UnExecute()
    {
        //empty
    }
}