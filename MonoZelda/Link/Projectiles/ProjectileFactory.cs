using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using System;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileFactory
{
    public ProjectileFactory()
    {
    }

    private readonly Dictionary<ProjectileType, Vector2> projectileDimensions = new Dictionary<ProjectileType, Vector2>()
    {
        {ProjectileType.Arrow,new Vector2(32,64)},
        {ProjectileType.ArrowBlue,new Vector2(32,64)},
        {ProjectileType.Boomerang,new Vector2(32,32)},
        {ProjectileType.BoomerangBlue,new Vector2(32,32)},
        {ProjectileType.Bomb,new Vector2(32,64)},
        {ProjectileType.Fire,new Vector2(64,64)},
        {ProjectileType.WoodenSword,new Vector2(32,64)},
        {ProjectileType.WoodenSwordBeam,new Vector2(32,64)},
    };

    protected Vector2 GetSpawnPosition(Vector2 Dimension)
    {
        Vector2 positionInitializer = new Vector2();
        Vector2 projectilePosition = new Vector2();
        switch (PlayerState.Direction)
        {
            case Direction.Up:
                positionInitializer.Y = (-(Dimension.Y / 2)) - 32;
                break;
            case Direction.Down:
                positionInitializer.Y = ((Dimension.Y / 2)) + 32; ;
                break;
            case Direction.Left:
                positionInitializer.X = -((Dimension.X / 2)) - 32;
                break;
            case Direction.Right:
                positionInitializer.X = ((Dimension.X / 2)) + 32;
                break;
        }
        projectilePosition = PlayerState.Position.ToVector2() + positionInitializer;
        return projectilePosition;
    }

    public IProjectile GetProjectileObject(ProjectileType requestedProjectile, CollisionController collisionController)
    {
        // get projectile initial spawn position
        Vector2 projectileSpawnPos = GetSpawnPosition(projectileDimensions[requestedProjectile]);

        // create projectile object
        var projectileType = Type.GetType($"MonoZelda.Link.Projectiles.{requestedProjectile}");
        IProjectile projectile = (IProjectile)Activator.CreateInstance(projectileType,projectileSpawnPos,collisionController);

        return projectile;
    }
}
