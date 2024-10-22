using Microsoft.Xna.Framework.Input;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private Player player;
    private Projectile projectile;
    private ProjectileManager projectileManager;
    private IProjectile launchedProjectile;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(Projectile projectile, ProjectileManager projectileManager, Player player)
    {
        this.player = player;
        this.projectile = projectile;
        this.projectileManager = projectileManager;
    }

    private void CreateProjectile(Keys PressedKey)
    {
        launchedProjectile = projectile.GetProjectileObject(projectileManager.getProjectileType(PressedKey));
        projectileManager.SetProjectile(launchedProjectile);
        projectile.EnableDict();
    }

    public void Execute(params object[] metadata)
    {
        Keys pressedKey = (Keys)metadata[0];
        CreateProjectile(pressedKey);
        player?.Attack();
    }

    public void UnExecute()
    {
        //empty
    }
}
