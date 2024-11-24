using MonoZelda.Sound;

namespace MonoZelda.Link.Equippables;

public class RedPotionEquippable : IEquippable
{
    public RedPotionEquippable()
    {
        // empty
    }

    public void Use(params object[] args)
    {
        SoundManager.PlaySound("LOZ_Drink_Potion", false);
        PlayerState.Health = PlayerState.MaxHealth;
        PlayerState.EquippableInventory.Remove(EquippableType.RedPotion);
    }
}
