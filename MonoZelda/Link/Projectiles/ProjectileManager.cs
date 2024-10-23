﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private bool projectileFired;
    private IProjectile itemFired;
    private ProjectileType equippedProjectile;
    private CollisionController collisionController;
    private Collidable projectileCollidable;
    private GraphicsDevice graphicsDevice;
    private Projectile projectile;
    private SpriteDict projectileDict;

    private Dictionary<Keys, ProjectileType> keyProjectileMap = new Dictionary<Keys, ProjectileType>
    {
        {Keys.D1,ProjectileType.Arrow},
        {Keys.D2,ProjectileType.ArrowBlue},
        {Keys.D3,ProjectileType.Boomerang},
        {Keys.D4,ProjectileType.BoomerangBlue},
        {Keys.D5,ProjectileType.Bomb},
        {Keys.D6,ProjectileType.CandleBlue},
    };
    
    public bool ProjectileFired
    {
        get
        {
            return projectileFired;
        }
        set
        {
            projectileFired = value;
        }
    }

    public ProjectileManager(CollisionController collisionController, GraphicsDevice graphicsDevice, SpriteDict projectileDict) 
    {
        projectileFired = false;
        projectileDict.Enabled = false;
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
        this.projectileDict = projectileDict;
        projectile = new Projectile(projectileDict, new Vector2(),Direction.Down);
    }

    private void setupProjectile()
    {
        projectileFired = true;
        projectileDict.Enabled = true;
        projectileCollidable = new Collidable(itemFired.getCollisionRectangle(), graphicsDevice, CollidableType.Projectile);
        projectileCollidable.setProjectileManager(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void setProjectile(Keys pressedKey)
    {
        equippedProjectile = keyProjectileMap[pressedKey];
    }
    
    public void destroyProjectile()
    {
        itemFired.FinishProjectile();
    }

    public void useSword(Player player)
    {
        itemFired = projectile.GetProjectileObject(ProjectileType.WoodenSword, player);
        setupProjectile();
    }

    public void useSwordBeam(Player player)
    {
        itemFired = projectile.GetProjectileObject(ProjectileType.WoodenSwordBeam, player);
        setupProjectile();
    }

    public void fireEquippedProjectile(Player player)
    {
        itemFired = projectile.GetProjectileObject(equippedProjectile, player);
        setupProjectile();
    }

    public void executeProjectile()
    {
        if (itemFired != null && !itemFired.hasFinished())
        {
            itemFired.UpdateProjectile();
            projectileCollidable.Bounds = itemFired.getCollisionRectangle();
        }
        else
        {
            collisionController.RemoveCollidable(projectileCollidable);
            projectileCollidable.UnregisterHitbox();
            projectileFired = false;
        }        
    }
}
