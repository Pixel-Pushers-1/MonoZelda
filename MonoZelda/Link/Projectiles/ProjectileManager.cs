using Microsoft.Xna.Framework.Input;
using MonoZelda.Controllers;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private CollisionController collisionController;
    private ProjectileFactory projectileFactory;
    private List<IProjectile> projectiles;

    private readonly Dictionary<WeaponType, ProjectileType> weaponProjectileMap = new()
    {
        {WeaponType.Bow, ProjectileType.Arrow},
        {WeaponType.Boomerang, ProjectileType.Boomerang},
        {WeaponType.CandleBlue, ProjectileType.Fire },
        {WeaponType.Bomb, ProjectileType.Bomb },
    };

    public WeaponType EquippedWeapon
    {
        get => PlayerState.EquippedWeapon;
        private set => PlayerState.EquippedWeapon = value;
    }

    public ProjectileManager(CollisionController collisionController)
    {
        EquippedWeapon = WeaponType.None;
        projectiles = new List<IProjectile>();
        this.collisionController = collisionController;
        projectileFactory = new ProjectileFactory();
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

    public void CycleWeapon()
    {
        EquippedWeapon = EquippedWeapon + 1;
        if(EquippedWeapon == WeaponType.Bomb)
        {
            EquippedWeapon = WeaponType.None;
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

    public void FireProjectile()
    {
        // check if player has equipped a weapon
        if (PlayerState.EquippedWeapon == WeaponType.None)
        {
            return;
        }

        // create projectile
        ProjectileType projectileType = weaponProjectileMap[PlayerState.EquippedWeapon];
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