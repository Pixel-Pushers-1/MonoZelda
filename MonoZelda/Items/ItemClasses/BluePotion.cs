using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using Microsoft.Xna.Framework;
using MonoZelda.Link.Equippables;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : Item
{
    public BluePotion(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.BluePotion;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        base.ItemSpawn(itemSpawn, collisionController); 
        itemDict.SetSprite("potion_blue");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.EquippableManager.AddEquippable(EquippableType.BluePotion, false);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}