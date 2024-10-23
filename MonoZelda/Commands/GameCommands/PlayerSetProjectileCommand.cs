using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerSetProjectileCommand : ICommand
{
    private Player player;
    private ProjectileManager projectileManager;

    public PlayerSetProjectileCommand()
    {
        //empty
    }

    public PlayerSetProjectileCommand(ProjectileManager projectileManager, Player player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        Keys pressedKey = (Keys)metadata[0];
        projectileManager.equipProjectile(pressedKey);
    }

    public void UnExecute()
    {
        //empty
    }
}