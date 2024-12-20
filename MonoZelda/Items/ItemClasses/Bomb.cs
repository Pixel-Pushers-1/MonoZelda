﻿using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using Microsoft.Xna.Framework;
using MonoZelda.Link.Equippables;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : Item
{
    public Bomb(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Bomb;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        base.ItemSpawn(itemSpawn, collisionController);  
        itemDict.SetSprite("bomb");   
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddBombs(1);
        PlayerState.EquippableManager.AddEquippable(EquippableType.Bomb, false);

        // play sound and handle collision
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}