using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class Projectile
{

    private ProjectileType currentProjectile;
    private SpriteDict projectileDict;
    private Player player;
    protected Vector2 projectilePosition;
    protected Direction playerDirection;

    public Projectile(SpriteDict projectileDict,Player player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        projectilePosition = new Vector2();
    }

    protected void SetProjectileSprite(string projectileName)
    {
        projectileDict.SetSprite(projectileName);
    }

    protected double CalculateDistance(Vector2 initialPosition)
    {
        return Math.Sqrt(Math.Pow(projectilePosition.X - initialPosition.X, 2) + Math.Pow(projectilePosition.Y - initialPosition.Y, 2));
    }

    protected Vector2 SetInitialPosition(Vector2 Dimension)
    {
        playerDirection = player.PlayerDirection;
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
        projectilePosition = player.GetPlayerPosition() + positionInitializer;
        return projectilePosition;
    }

    public IProjectile GetProjectileObject(ProjectileType currentProjectile)
    {
        var projectileType = Type.GetType($"MonoZelda.Link.Projectiles.{currentProjectile}");
        IProjectile launchProjectile = (IProjectile)Activator.CreateInstance(projectileType, projectileDict, player);

        return launchProjectile;
    }

    public void EnableDict()
    {
        projectileDict.Enabled = true;
    }
}
