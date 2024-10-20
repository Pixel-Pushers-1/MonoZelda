using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private bool projectileFired;
    private IProjectile itemFired;
    private CollisionController collisionController;
    private Collidable projectileCollidable;
    private GraphicsDevice graphicsDevice;

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

    public ProjectileManager(CollisionController collisionController, GraphicsDevice graphicsDevice) 
    {
        projectileFired = false;
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }
    
    public ProjectileType getProjectileType(Keys PressedKey)
    {
        return keyProjectileMap[PressedKey];
    }

    public void SetProjectile(IProjectile projectile)
    {
        itemFired = projectile;
        projectileFired = true;
        projectileCollidable = new Collidable(projectile.getCollisionRectangle(),graphicsDevice, CollidableType.Projectile);
        collisionController.AddCollidable(projectileCollidable);
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
