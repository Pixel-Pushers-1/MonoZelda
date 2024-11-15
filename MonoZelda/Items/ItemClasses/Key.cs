using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Key : Item
{
    public Key(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Key;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);    
        itemDict.SetSprite("key_0");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddKeys(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
