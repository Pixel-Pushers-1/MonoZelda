using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class Projectile
{
    private SpriteDict projectileDict;
    protected Vector2 projectilePosition;
    protected Vector2 playerPosition;
    protected Direction playerDirection;

    public Projectile(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    {
        this.projectileDict = projectileDict;
        this.playerPosition = playerPosition;
        this.playerDirection = playerDirection; 
    }

    protected void SetProjectileSprite(string projectileName)
    {
        projectileDict.SetSprite(projectileName);
    }

    protected Vector2 SetInitialPosition(Vector2 Dimension)
    {
        Vector2 positionInitializer = new Vector2();
        switch (playerDirection)
        {
            case Direction.Up:
                positionInitializer.Y = (-(Dimension.Y / 2) * 4) - 32;
                break;
            case Direction.Down:
                positionInitializer.Y = ((Dimension.Y / 2) * 4) + 32; ;
                break;
            case Direction.Left:
                positionInitializer.X = -((Dimension.X / 2) * 4) - 32;
                break;
            case Direction.Right:
                positionInitializer.X = ((Dimension.X / 2) * 4) + 32;
                break;
        }
        projectilePosition = playerPosition + positionInitializer;
        return projectilePosition;
    }

    public IProjectile GetProjectileObject(ProjectileType currentProjectile, Player player)
    {
        playerPosition = player.GetPlayerPosition();
        playerDirection = player.PlayerDirection;
        var projectileType = Type.GetType($"MonoZelda.Link.Projectiles.{currentProjectile}");

        // Get the constructor with the parameters projectileDict and player, if it exists
        var constructorWithPlayer = projectileType.GetConstructor(new[] { typeof(SpriteDict), typeof(Vector2), typeof(Direction), typeof(Player) });

        IProjectile launchProjectile;

        if (constructorWithPlayer != null)
        {
            // Use constructor with player
            launchProjectile = (IProjectile)Activator.CreateInstance(projectileType, projectileDict, playerPosition, playerDirection, player);
        }
        else
        {
            // Use constructor without player
            launchProjectile = (IProjectile)Activator.CreateInstance(projectileType, projectileDict, playerPosition, playerDirection);
        }

        return launchProjectile;
    }
}
