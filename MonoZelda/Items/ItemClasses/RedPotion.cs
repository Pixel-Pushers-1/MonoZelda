using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class RedPotion : Item
{
    public RedPotion(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.RedPotion;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("potion");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        if (!PlayerState.UtilityInventory.ContainsKey(WeaponType.RedPotion))
        {
            PlayerState.UtilityInventory.Add(WeaponType.RedPotion, true);
        }
        else
        {
            PlayerState.UtilityInventory[WeaponType.RedPotion] = true;
        }
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
