using MonoZelda.Link.Projectiles;

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
        EquippableManager equippableManager = (EquippableManager)args[1];

        projectileManager.FireProjectile(ProjectileType.Bomb);
        if(PlayerState.Bombs == 0)
        {
            equippableManager.RemoveEquippable(EquippableType.Bomb);
        }
    }
}
