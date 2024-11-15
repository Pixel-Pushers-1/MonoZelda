using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : Item
{
    public Rupee(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Rupee;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("rupee");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddRupees(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}