using System;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using PixelPushers.MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;


namespace PixelPushers.MonoZelda.Commands;

public class PlayerUseItemCommand : ICommand
{
    private Player player;
    private Projectile projectiles;
    private ProjectileManager projectileManager;
    private IController controller;
    private IProjectile launchedProjectile;

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

    public GameState Execute(Keys PressedKey)
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

        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }
}
