using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Link.Projectiles.Arrows;
using MonoZelda.Link.Projectiles.Explosive;
using MonoZelda.Link.Projectiles.Fire;
using MonoZelda.Link.Projectiles.Boomerangs;
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
        Vector2 offset = new Vector2(0, 0);
        switch (playerDirection)
        {
            case Direction.Up:
                offset = new Vector2(0, -(Dimension.Y / 2) * 4);
                projectilePosition = player.GetPlayerPosition() + (new Vector2(0, -32)) + offset;
                break;
            case Direction.Down:
                offset = new Vector2(0, (Dimension.Y / 2) * 4);
                projectilePosition = player.GetPlayerPosition() + (new Vector2(0, 32)) + offset;
                break;
            case Direction.Left:
                offset = new Vector2(-(Dimension.X / 2) * 4, 0);
                projectilePosition = player.GetPlayerPosition() + (new Vector2(-32, 0)) + offset;
                break;
            case Direction.Right:
                offset = new Vector2((Dimension.X / 2) * 4, 0);
                projectilePosition = player.GetPlayerPosition() + (new Vector2(32, 0)) + offset;
                break;
        }
        return projectilePosition;
    }

    public void EnableDict()
    {
        projectileDict.Enabled = true;
    }


    public IProjectile GetProjectileObject(ProjectileType currentProjectile)
    {
        IProjectile launchProjectile = null;
        switch (currentProjectile)
        {
            case ProjectileType.arrow_green:
                launchProjectile = new Arrow(projectileDict,player);
                break;
            case ProjectileType.arrow_blue:
                launchProjectile = new ArrowBlue(projectileDict,player);
                break;
            case ProjectileType.boomerang_green:
                launchProjectile = new Boomerang(projectileDict,player);
                break;
            case ProjectileType.boomerang_blue:
                launchProjectile = new BoomerangBlue(projectileDict, player);
                break;
            case ProjectileType.bomb:
                launchProjectile = new Bomb(projectileDict,player);
                break;
            case ProjectileType.candle_blue:
                launchProjectile = new CandleBlue(projectileDict,player);
                break;
        }
        return launchProjectile;
    }
}
