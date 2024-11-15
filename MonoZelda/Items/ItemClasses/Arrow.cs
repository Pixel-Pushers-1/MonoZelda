using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Arrow : Item
{
    public Arrow(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Arrow;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("arrow");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}