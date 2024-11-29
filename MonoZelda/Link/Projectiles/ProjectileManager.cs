using MonoZelda.Controllers;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private CollisionController collisionController;
    private ProjectileFactory projectileFactory;
    private Dictionary<ProjectileType, IProjectile> ActiveProjectiles;

    private List<ProjectileType> SingleInstanceProjectiles = new()
    {
        {ProjectileType.Arrow},
        {ProjectileType.Boomerang},
    };
    
    public ProjectileManager(CollisionController collisionController)
    {
        ActiveProjectiles = new Dictionary<ProjectileType, IProjectile>();
        projectileFactory = new ProjectileFactory();
        this.collisionController = collisionController;
    }

    private void DeductPlayerResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
                PlayerState.Rupees--;
                break;
            case ProjectileType.Bomb:
                PlayerState.Bombs--;
                break;
            case ProjectileType.Fire:
                PlayerState.IsCandleUsed = true;
                break;
        }
    }

    private bool HasRequiredResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
                return PlayerState.Rupees > 0;
            case ProjectileType.Bomb:
                return PlayerState.Bombs > 0;
            case ProjectileType.Fire:
                return !PlayerState.IsCandleUsed;
            default:
                return true;
        }
    }

    public void UseSword()
    {
        IProjectile sword;
        if (PlayerState.Health == PlayerState.MaxHealth && ActiveProjectiles.ContainsKey(ProjectileType.WoodenSwordBeam) == false)
        {
            sword = projectileFactory.GetProjectileObject(ProjectileType.WoodenSwordBeam,collisionController);
            ActiveProjectiles.Add(ProjectileType.WoodenSwordBeam, sword);
        }
        else
        {
            sword = projectileFactory.GetProjectileObject(ProjectileType.WoodenSword,collisionController);
            ActiveProjectiles.Add(ProjectileType.WoodenSword, sword);
        }
        sword.Setup();
    }

    public void FireProjectile(ProjectileType projectileType)
    {
        if (SingleInstanceProjectiles.Contains(projectileType))
        {
            if (HasRequiredResources(projectileType) && ActiveProjectiles.ContainsKey(projectileType) == false)
            {
                IProjectile projectile = projectileFactory.GetProjectileObject(projectileType, collisionController);
                ActiveProjectiles.Add(projectileType, projectile);
                DeductPlayerResources(projectileType);
                projectile.Setup();
            }
        }
        else
        {
            if (HasRequiredResources(projectileType))
            {
                IProjectile projectile = projectileFactory.GetProjectileObject(projectileType, collisionController);
                ActiveProjectiles.Add(projectileType, projectile);
                DeductPlayerResources(projectileType);
                projectile.Setup();
            }
        }
    }

    public void Update()
    {
        var Projectiles = new List<ProjectileType>(ActiveProjectiles.Keys);
        
        // loop over all active projectile objects
        foreach(var projectile in Projectiles)
        {
            IProjectile projectileInstance = ActiveProjectiles[projectile];
            projectileInstance.Update();
            if (projectileInstance.hasFinished())
            {
                ActiveProjectiles.Remove(projectile);
            }
        }
    }
}