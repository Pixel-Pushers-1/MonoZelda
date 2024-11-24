using MonoZelda.Controllers;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private CollisionController collisionController;
    private ProjectileFactory projectileFactory;
    private List<IProjectile> projectiles;
    private bool BOOMERANG_ACTIVE;
    private bool SWORD_BEAM_ACTIVE;

    public ProjectileManager(CollisionController collisionController)
    {
        projectiles = new List<IProjectile>();
        this.collisionController = collisionController;
        projectileFactory = new ProjectileFactory();
        BOOMERANG_ACTIVE = false;
        SWORD_BEAM_ACTIVE = false;
    }

    private bool HasRequiredResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
            case ProjectileType.ArrowBlue:
                return PlayerState.Rupees > 0;
            case ProjectileType.Fire:
                return !PlayerState.IsCandleUsed;
            case ProjectileType.Bomb:
                return PlayerState.Bombs > 0;
            default:
                return true;
        }
    }

    private void DeductResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
            case ProjectileType.ArrowBlue:
                PlayerState.Rupees--;
                break;
            case ProjectileType.Fire:
                PlayerState.IsCandleUsed = true;
                break;
            case ProjectileType.Bomb:
                PlayerState.Bombs--;
                break;
        }
    }

    public void UseSword()
    {
        IProjectile sword;
        if(PlayerState.Health == PlayerState.MaxHealth)
        {
            sword = projectileFactory.GetProjectileObject(ProjectileType.WoodenSwordBeam,collisionController);
        }
        else
        {
            sword = projectileFactory.GetProjectileObject(ProjectileType.WoodenSword,collisionController);
        }
        sword.Setup();
        projectiles.Add(sword);
    }

    public void FireProjectile(ProjectileType projectileType)
    {
        if (HasRequiredResources(projectileType))
        {
            DeductResources(projectileType);
            IProjectile projectile = projectileFactory.GetProjectileObject(projectileType, collisionController);
            projectile.Setup();
            projectiles.Add(projectile);
        }
    }

    public void Update()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            IProjectile projectile = projectiles[i];
            projectile.Update();
            if (projectile.hasFinished())
            {
                projectiles.Remove(projectile);
            }
        }
    }
}