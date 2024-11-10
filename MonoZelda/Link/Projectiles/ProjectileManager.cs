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
    private ProjectileType equippedProjectile;
    private PlayerProjectileCollidable projectileCollidable;
    private CollisionController collisionController;
    private GraphicsDevice graphicsDevice;
    private ProjectileFactory projectileFactory;
    private SpriteDict projectileDict;
    private bool activateHitbox;

    public event Action<PlayerProjectileCollidable> OnProjectileColliderActive;

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
        activateHitbox = false;
        projectileDict.Enabled = false;
        equippedProjectile = PlayerState.EquippedProjectile;
        this.collisionController = collisionController;
        this.projectileDict = projectileDict;
        projectileFactory = new ProjectileFactory(projectileDict, new Vector2(),Direction.Down);
    }

    private void setupProjectile(ProjectileType equippedProjectile)
    {
        projectileFired = true;
        projectileDict.Enabled = true;
        if (equippedProjectile != ProjectileType.Bomb) {
            activateHitbox = true;
        }
    }

    public void equipProjectile(Keys pressedKey)
    {
        if (keyProjectileMap.TryGetValue(pressedKey, out ProjectileType newProjectile))
        {
            EquippedProjectile = newProjectile;  // Use the property instead of field
            PlayerState.EquippedProjectile = equippedProjectile;
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

    public void UpdatedProjectileState()
    {
        if (itemFired != null && !itemFired.hasFinished())
        {
            // Init projectile collidable
            if (activateHitbox && projectileCollidable is null) {
                projectileCollidable = new PlayerProjectileCollidable(itemFired.getCollisionRectangle(), equippedProjectile);
                projectileCollidable.setProjectileManager(this);
                collisionController.AddCollidable(projectileCollidable);
                OnProjectileColliderActive?.Invoke(projectileCollidable);
            } else if (activateHitbox && projectileCollidable is not null)
            {
                projectileCollidable.Bounds = itemFired.getCollisionRectangle();
            }

            // Update the projectile
            itemFired.UpdateProjectile();

            // Custom check for bombs (I know this isn't great practice)
            if (itemFired is Bomb && projectileCollidable is null) {
                Bomb itemFiredCls = itemFired as Bomb;
                if (itemFiredCls.Exploded == true) {
                    activateHitbox = true;
                }
            }
            
        }
        else
        {
            // Reset the projectile manager for the next projectile
            if (projectileCollidable is not null) {
                collisionController.RemoveCollidable(projectileCollidable);
                projectileCollidable.UnregisterHitbox();
                projectileCollidable = null;
            }
            projectileFired = false;
            activateHitbox = false;
        }
    }
}