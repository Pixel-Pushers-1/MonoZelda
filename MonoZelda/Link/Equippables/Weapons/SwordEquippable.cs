using MonoZelda.Link.Projectiles;

namespace MonoZelda.Link.Equippables;

public class SwordEquippable : IEquippable  
{
    public SwordEquippable()
    {
        // empty
    }

    public void Use(params object[] args)
    {
        ProjectileManager projectileManager = (ProjectileManager)args[0];
        projectileManager.UseSword();
    }
}
