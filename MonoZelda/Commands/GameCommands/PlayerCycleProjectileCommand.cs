using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;
using MonoZelda.HUD;

namespace MonoZelda.Commands.GameCommands;

public class PlayerCycleProjectileCommand : ICommand
{
    private ProjectileManager projectileManager;
    public PlayerCycleProjectileCommand()
    {
        //empty
    }

    public PlayerCycleProjectileCommand(ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        int cycleAmount = (int)metadata[0];
        projectileManager.cycleProjectile(cycleAmount);
    }

    public void UnExecute()
    {
        //empty
    }
}