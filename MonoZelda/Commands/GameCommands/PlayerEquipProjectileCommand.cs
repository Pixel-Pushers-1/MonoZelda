using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerEquipProjectileCommand : ICommand
{
    private PlayerSpriteManager player;
    private ProjectileManager projectileManager;

    public PlayerEquipProjectileCommand()
    {
        //empty
    }

    public PlayerEquipProjectileCommand(ProjectileManager projectileManager, PlayerSpriteManager player)
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