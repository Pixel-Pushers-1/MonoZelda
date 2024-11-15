﻿using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class BlueCandle : Item
{
    public BlueCandle(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.BlueCandle;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("candle_blue");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}

