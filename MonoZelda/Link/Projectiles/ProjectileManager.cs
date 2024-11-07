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
    private PlayerProjectileCollidable projectileCollidable;
    private CollisionController collisionController;
    private GraphicsDevice graphicsDevice;
    private ProjectileFactory projectileFactory;
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
        get => projectileFired;
        set => projectileFired = value;
    }

    public ProjectileType EquippedProjectile
    {
        get => equippedProjectile;
        private set => equippedProjectile = value;
    }

    public ProjectileManager(CollisionController collisionController, SpriteDict projectileDict)
    {
        projectileFired = false;
        projectileDict.Enabled = false;
        this.collisionController = collisionController;
        this.projectileDict = projectileDict;
        projectileFactory = new ProjectileFactory(projectileDict, new Vector2(),Direction.Down);
    }

    private void setupProjectile(ProjectileType equippedProjectile)
    {
        projectileFired = true;
        projectileDict.Enabled = true;
        projectileCollidable = new PlayerProjectileCollidable(itemFired.getCollisionRectangle(), equippedProjectile);
        projectileCollidable.setProjectileManager(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void equipProjectile(Keys pressedKey)
    {
        if (keyProjectileMap.TryGetValue(pressedKey, out ProjectileType newProjectile))
        {
            EquippedProjectile = newProjectile;  // Use the property instead of field
        }
    }

    public void destroyProjectile()
    {
        itemFired.FinishProjectile();
    }

    public void useSword(PlayerSpriteManager player)
    {
        itemFired = projectileFactory.GetProjectileObject(ProjectileType.WoodenSword, player);
        setupProjectile(ProjectileType.WoodenSword);
    }

    public void useSwordBeam(PlayerSpriteManager player)
    {
        itemFired = projectileFactory.GetProjectileObject(ProjectileType.WoodenSwordBeam, player);
        setupProjectile(ProjectileType.WoodenSwordBeam);
    }

    public void fireEquippedProjectile(PlayerSpriteManager player)
    {
        itemFired = projectileFactory.GetProjectileObject(EquippedProjectile, player);  // Use the property instead of field
        setupProjectile(EquippedProjectile);  // Use the property instead of field
    }

    public void updatedProjectileState()
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