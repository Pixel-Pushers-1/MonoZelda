using Microsoft.Xna.Framework.Input;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private Player player;
    private ProjectileManager projectileManager;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(ProjectileManager projectileManager, Player player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        // use sword projectile
        if (projectileManager.ProjectileFired == false)
        {
            projectileManager.useSword(player);
            player?.Attack();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
