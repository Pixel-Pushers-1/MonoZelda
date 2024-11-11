using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private bool projectileFired;
    private IProjectile itemFired;
    private PlayerProjectileCollidable projectileCollidable;
    private CollisionController collisionController;
    private GraphicsDevice graphicsDevice;
    private ProjectileFactory projectileFactory;
    private SpriteDict projectileDict;
    private bool activateHitbox;
    private bool isSwordAttack;
    private bool isSwordBeamAttack;

    private readonly Dictionary<Keys, ProjectileType> keyProjectileMap = new Dictionary<Keys, ProjectileType>
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
        get => PlayerState.EquippedProjectile;
        private set => PlayerState.EquippedProjectile = value;
    }

    public ProjectileManager(CollisionController collisionController, SpriteDict projectileDict)
    {
        projectileFired = false;
        activateHitbox = false;
        isSwordAttack = false;
        isSwordBeamAttack = false;
        projectileDict.Enabled = false;
        this.collisionController = collisionController;
        this.projectileDict = projectileDict;
        projectileFactory = new ProjectileFactory(projectileDict, new Vector2(), Direction.Down);
    }

    private bool hasRequiredResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
            case ProjectileType.ArrowBlue:
                return PlayerState.Rupees > 0;
            case ProjectileType.Bomb:
                return PlayerState.Bombs > 0;
            default:
                return true;
        }
    }

    private void deductResources(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Arrow:
            case ProjectileType.ArrowBlue:
                PlayerState.Rupees--;
                break;
            case ProjectileType.Bomb:
                PlayerState.Bombs--;
                break;
        }
    }

    private void setupProjectile(ProjectileType equippedProjectile)
    {
        if (!hasRequiredResources(equippedProjectile))
        {
            return;
        }
        
        if (equippedProjectile == ProjectileType.WoodenSword)
        {
            isSwordAttack = true;
        }
        else if (equippedProjectile == ProjectileType.WoodenSwordBeam)
        {
            isSwordBeamAttack = true;
        }

        deductResources(equippedProjectile);
        projectileFired = true;
        projectileDict.Enabled = true;
        if (equippedProjectile != ProjectileType.Bomb)
        {
            activateHitbox = true;
        }
    }

    public void equipProjectile(Keys pressedKey)
    {
        if (keyProjectileMap.TryGetValue(pressedKey, out ProjectileType newProjectile))
        {
            EquippedProjectile = newProjectile;
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
        if (!hasRequiredResources(EquippedProjectile))
        {
            return;
        }
        //prevent candle use in more than one room
        if (EquippedProjectile == ProjectileType.CandleBlue && PlayerState.IsCandleUsed)
        {
            return; 
        }

        itemFired = projectileFactory.GetProjectileObject(EquippedProjectile, player);
        setupProjectile(EquippedProjectile);

        if (EquippedProjectile == ProjectileType.CandleBlue)
        {
            PlayerState.IsCandleUsed = true;
        }
    }

    public void UpdatedProjectileState()
    {
        if (itemFired != null && !itemFired.hasFinished())
        {
            // Init projectile collidable
            if (activateHitbox && projectileCollidable is null)
            {
                if (isSwordAttack)
                {
                    projectileCollidable = new PlayerProjectileCollidable(itemFired.getCollisionRectangle(), ProjectileType.WoodenSword);
                    isSwordAttack = false;
                }
                else if (isSwordBeamAttack)
                {
                    projectileCollidable = new PlayerProjectileCollidable(itemFired.getCollisionRectangle(), ProjectileType.WoodenSwordBeam);
                    isSwordBeamAttack = false;
                }
                else
                {
                    projectileCollidable = new PlayerProjectileCollidable(itemFired.getCollisionRectangle(), EquippedProjectile);

                }
                projectileCollidable.setProjectileManager(this);
                collisionController.AddCollidable(projectileCollidable);
            }
            else if (activateHitbox && projectileCollidable is not null)
            {
                projectileCollidable.Bounds = itemFired.getCollisionRectangle();
            }

            // Update the projectile
            itemFired.UpdateProjectile();

            // Custom check for bombs (I know this isn't great practice)
            if (itemFired is Bomb && projectileCollidable is null)
            {
                Bomb itemFiredCls = itemFired as Bomb;
                if (itemFiredCls.Exploded == true)
                {
                    activateHitbox = true;
                }
            }
        }
        else
        {
            // Reset the projectile manager for the next projectile
            if (projectileCollidable is not null)
            {
                collisionController.RemoveCollidable(projectileCollidable);
                projectileCollidable.UnregisterHitbox();
                projectileCollidable = null;
            }
            projectileFired = false;
            activateHitbox = false;
        }
    }
}