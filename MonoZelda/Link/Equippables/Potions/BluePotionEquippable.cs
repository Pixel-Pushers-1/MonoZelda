using MonoZelda.Link.Projectiles;
using MonoZelda.Sound;

namespace MonoZelda.Link.Equippables;

public class BluePotionEquippable : IEquippable
{
    public BluePotionEquippable()
    {
        //empty
    }

    public void Use(params object[] args)
    {
        SoundManager.PlaySound("LOZ_Drink_Potion", false);
        PlayerState.Health = PlayerState.MaxHealth;
    }
}
