using MonoZelda.Link.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Link.Equippables;

public class BombEquippable : IEquippable
{
    public BombEquippable()
    {
        // empty
    }

    public void Use(params object[] args)
    {
        ProjectileManager projectileManager = (ProjectileManager)args[0];
        projectileManager.FireProjectile(ProjectileType.Bomb);
    }
}
