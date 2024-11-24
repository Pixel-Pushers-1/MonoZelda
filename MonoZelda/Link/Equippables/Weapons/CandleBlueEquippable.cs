using MonoZelda.Link.Projectiles;

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
        projectileManager.FireProjectile(ProjectileType.Fire);
    }
}
