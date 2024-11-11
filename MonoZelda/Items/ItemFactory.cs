using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using MonoZelda.Items.ItemClasses;
using System;

namespace MonoZelda.Items;

public class ItemFactory
{ 
    private CollisionController collisionController;

    public ItemFactory( CollisionController collisionController)
    {
        this.collisionController = collisionController;
    }

    public void CreateItem(ItemList itemName, Point spawnPosition)
    {
        var itemDict = new SpriteDict(SpriteType.Items, 0, new Point(0, 0));
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemName}");
        Item item = (Item)Activator.CreateInstance(itemType);
        item.ItemSpawn(itemDict, spawnPosition, collisionController);
    }
}

