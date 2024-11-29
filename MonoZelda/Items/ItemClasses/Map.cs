using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.UI;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Map : Item
{
    public Map(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Map;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        base.ItemSpawn(itemSpawn, collisionController);        
        itemDict.SetSprite("map");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.HasMap = true;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}

