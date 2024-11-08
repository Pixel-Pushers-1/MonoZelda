using Microsoft.Xna.Framework.Input;
using MonoZelda.Link.Projectiles;
using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerFireSwordBeamCommand : ICommand
{
    private PlayerSpriteManager player;
    private ProjectileManager projectileManager;

    public PlayerFireSwordBeamCommand()
    {
        //empty
    }

    public PlayerFireSwordBeamCommand(ProjectileManager projectileManager, PlayerSpriteManager player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        // fire sword beam
        if(projectileManager.ProjectileFired == false)
        {
            projectileManager.useSwordBeam(player);
            player?.Attack();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
