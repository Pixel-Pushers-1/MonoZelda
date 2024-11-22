using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerFireProjectileCommand : ICommand
{
    private PlayerSpriteManager player;
    private ProjectileManager projectileManager;

    public PlayerFireProjectileCommand()
    {
        //empty
    }

    public PlayerFireProjectileCommand(ProjectileManager projectileManager, PlayerSpriteManager player)
    {
        this.player = player;
        this.projectileManager = projectileManager;
    }

    public void Execute(params object[] metadata)
    {
        Keys pressedKey = (Keys)metadata[0];

        // fire projectile according to equipped weapon
        projectileManager.FireProjectile();
        player?.UseItem();
    }

    public void UnExecute()
    {
        //empty
    }
}
