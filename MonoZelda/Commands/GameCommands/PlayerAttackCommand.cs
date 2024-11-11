using Microsoft.Xna.Framework.Input;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private PlayerSpriteManager player;
    private ProjectileManager projectileManager;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(ProjectileManager projectileManager, PlayerSpriteManager player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        if (PlayerState.IsMaxHealth())
        {
            // fire sword beam
            if (projectileManager.ProjectileFired == false)
            {
                projectileManager.useSwordBeam(player);
                player?.Attack();
            }
        }
        else
        {
            // use sword projectile
            if (projectileManager.ProjectileFired == false)
            {
                projectileManager.useSword(player);
                player?.Attack();
            }
        }
        
    }

    public void UnExecute()
    {
        //empty
    }
}
