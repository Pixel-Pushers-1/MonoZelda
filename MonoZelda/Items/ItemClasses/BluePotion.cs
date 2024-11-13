using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : Item
{
    public BluePotion(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.BluePotion;
    }

    public override void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(spawnPosition, collisionController); 
        itemDict.SetSprite("potion_blue");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.Health = PlayerState.MaxHealth;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}