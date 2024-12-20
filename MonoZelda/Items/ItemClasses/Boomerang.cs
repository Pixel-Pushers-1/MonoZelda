﻿using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using Microsoft.Xna.Framework;
using MonoZelda.Link.Equippables;
namespace MonoZelda.Items.ItemClasses;

public class Boomerang : Item
{
    public Boomerang(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Boomerang;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(32, 32));
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("boomerang");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.EquippableManager.AddEquippable(EquippableType.Boomerang, false);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
