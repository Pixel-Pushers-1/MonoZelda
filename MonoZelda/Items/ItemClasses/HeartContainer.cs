using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class HeartContainer : Item
{
    public HeartContainer(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.HeartContainer;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController); 
        itemDict.SetSprite("heartcontainer");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.MaxHealth += 2;
        PlayerState.Health += 2;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }

}
