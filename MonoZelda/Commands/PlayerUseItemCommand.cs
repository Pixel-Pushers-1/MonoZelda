using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerUseItemCommand : ICommand
{
    private Player player;
    private Projectile projectiles;
    private ProjectileManager projectileManager;
    private IProjectile launchedProjectile;

    public PlayerUseItemCommand()
    {
        //empty
    }

    public PlayerUseItemCommand(Projectile projectiles, ProjectileManager projectileManager, Player player)
    {
        this.player = player;
        this.projectiles = projectiles;
        this.projectileManager = projectileManager;
    }

    private void CreateProjectile(Keys PressedKey)
    {
        launchedProjectile = projectiles.GetProjectileObject(projectileManager.getProjectileType(PressedKey));
        projectileManager.SetProjectile(launchedProjectile);
        projectiles.EnableDict();
    }

    public void Execute(Keys PressedKey)
    {
        // create projectile
        if(!projectileManager.ProjectileFired)
        {
            CreateProjectile(PressedKey);
        }

        // animate player throw projectile
        if (player != null)
        {
            player.UseItem();
            launchedProjectile.UpdateProjectile();
        }
    }

    public void UnExecute()
    {
        //empty
    }
}
