using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerFireProjectileCommand : ICommand
{
    private Player player;
    private ProjectileManager projectileManager;

    public PlayerFireProjectileCommand()
    {
        //empty
    }

    public PlayerFireProjectileCommand(ProjectileManager projectileManager, Player player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        Keys pressedKey = (Keys)metadata[0];

        // fire equipped projectile
        if (projectileManager.ProjectileFired == false)
        {
            projectileManager.fireEquippedProjectile(player);
            player?.UseItem();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
