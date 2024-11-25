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
        // Get equippableManager object
        EquippableManager equippableManager = (EquippableManager)args[1];

        // Use BluePotion
        SoundManager.PlaySound("LOZ_Drink_Potion", false);
        PlayerState.Health = PlayerState.MaxHealth;

        // Update PlayerState and CyclingIndex
        PlayerState.EquippableInventory.Remove(EquippableType.RedPotion);
        PlayerState.EquippedItem = EquippableType.None;
        equippableManager.CyclingIndex = 0;
    }
}
