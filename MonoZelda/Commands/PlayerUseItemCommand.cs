using System;
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
    private ProjectileType projectileType;

    public MonoZeldaGame Game { get; set; }

    public PlayerUseItemCommand()
    {
    }

    public PlayerUseItemCommand(Projectile projectiles, ProjectileManager projectileManager, Player player)
    {
        this.projectiles = projectiles;
        this.projectileManager = projectileManager;
        this.player = player;
    }

    private void CreateProjectile(Keys PressedKey)
    {
        launchedProjectile = projectiles.GetProjectileObject(projectileManager.getProjectileType(PressedKey));
        projectileManager.setProjectile(launchedProjectile);
        projectiles.enableDict();
    }

    public void Execute(Keys PressedKey)
    {
        // create projectile
        if(projectileManager.ProjectileFired != true)
        {
            CreateProjectile(PressedKey);
        }

        // animate player throw projectile
        if (player != null)
        {
            player.PlayerUseItem();
            launchedProjectile.updateProjectile();
        }
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
