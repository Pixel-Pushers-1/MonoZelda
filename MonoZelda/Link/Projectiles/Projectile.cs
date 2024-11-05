﻿using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class Projectile
{
    private SpriteDict projectileDict;
    protected Vector2 projectilePosition;
    protected Vector2 playerPosition;
    protected Direction playerDirection;

    private static readonly Dictionary<ProjectileType, Func<SpriteDict, Vector2, Direction, PlayerSpriteManager, IProjectile>> projectileConstructors = new()
    {
       { ProjectileType.Arrow, (dict, pos, dir, player) => new Arrow(dict, pos, dir) },
       { ProjectileType.ArrowBlue, (dict, pos, dir, player) => new ArrowBlue(dict, pos, dir) },
       { ProjectileType.Bomb, (dict, pos, dir, player) => new Bomb(dict, pos, dir) },
       { ProjectileType.Boomerang, (dict, pos, dir, player) => new Boomerang(dict, pos, dir, player) },
       { ProjectileType.BoomerangBlue, (dict, pos, dir, player) => new BoomerangBlue(dict, pos, dir, player) },
       { ProjectileType.CandleBlue, (dict,pos,dir,player) => new CandleBlue(dict, pos, dir) },
       { ProjectileType.WoodenSword, (dict,pos,dir,player) => new WoodenSword(dict, pos, dir) },
       { ProjectileType.WoodenSwordBeam, (dict,pos,dir,player) => new WoodenSwordBeam(dict, pos, dir) }
    };
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

    public IProjectile GetProjectileObject(ProjectileType currentProjectile, PlayerSpriteManager player)
    {
        playerPosition = player.GetPlayerPosition();
        playerDirection = player.PlayerDirection;

        // Check if the projectile type exists in the dictionary
        if (projectileConstructors.TryGetValue(currentProjectile, out var constructor))
        {
            // Use the constructor to create the projectile
            return constructor(projectileDict, playerPosition, playerDirection, player);
        }

        throw new ArgumentException($"Unknown projectile type: {currentProjectile}");
    }
}
