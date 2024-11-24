using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerCycleWeaponCommand : ICommand
{
    private ProjectileManager projectileManager;
    public PlayerCycleWeaponCommand()
    {
        //empty
    }

    public PlayerCycleWeaponCommand(ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        projectileManager.CycleWeapon();
    }

    public void UnExecute()
    {
        //empty
    }
}