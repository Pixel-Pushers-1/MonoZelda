using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class ProjectileManager
{
    private bool projectileFired;
    private IProjectile itemFired;

    private Dictionary<Keys, ProjectileType> keyProjectileMap = new Dictionary<Keys, ProjectileType>
    {
        {Keys.D1,ProjectileType.arrow_green},
        {Keys.D2,ProjectileType.arrow_blue},
        {Keys.D3,ProjectileType.boomerang_green},
        {Keys.D4,ProjectileType.boomerang_blue},
        {Keys.D5,ProjectileType.bomb},
        {Keys.D6,ProjectileType.candle_blue},
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

    public ProjectileManager() 
    {
        projectileFired = false;
    }
    
    public ProjectileType getProjectileType(Keys PressedKey)
    {
        return keyProjectileMap[PressedKey];
    }
    public void SetProjectile(IProjectile projectile)
    {
        itemFired = projectile;
        projectileFired = true;
    }

    public void executeProjectile()
    {
        if(itemFired != null)
        {
            if (!itemFired.hasFinished())
            {
                itemFired.UpdateProjectile();
            }
            else
            {
                projectileFired = false;
            }
        }
    }
}
