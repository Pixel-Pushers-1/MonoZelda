using MonoZelda.Link.Projectiles;

namespace MonoZelda.Link.Equippables;

public class BowEquippable : IEquippable
{
    public BowEquippable()
    {
        // emtpy
    }

    public void Use(params object[] args)
    {
        ProjectileManager projectileManager = (ProjectileManager)args[0];
        projectileManager.FireProjectile(ProjectileType.Arrow);
    }
}
