using Microsoft.Xna.Framework.Input;
using MonoZelda.Link.Projectiles;
using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerFireSwordBeam : ICommand
{
    private Player player;
    private Projectile projectile;
    private ProjectileManager projectileManager;
    private IProjectile launchedProjectile;

    public PlayerFireSwordBeam()
    {
        //empty
    }

    public PlayerFireSwordBeam(Projectile projectile, ProjectileManager projectileManager, Player player)
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
