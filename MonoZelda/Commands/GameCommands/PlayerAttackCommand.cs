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
        player?.Attack();
        projectileManager?.UseSword();
    }

    public void UnExecute()
    {
        //empty
    }
}
