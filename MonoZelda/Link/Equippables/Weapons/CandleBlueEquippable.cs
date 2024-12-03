using MonoZelda.Link.Projectiles;
using MonoZelda.Shaders;
using System.Collections.Generic;

namespace MonoZelda.Link.Equippables;

public class CandleBlueEquippable : IEquippable
{
    public CandleBlueEquippable()
    {
        // empty
    }

    public void Use(params object[] args)
    {
        ProjectileManager projectileManager = (ProjectileManager)args[0];
        List<ILight> lights = (List<ILight>)args[2];

        projectileManager.FireProjectile(ProjectileType.Fire, lights);
    }
}
