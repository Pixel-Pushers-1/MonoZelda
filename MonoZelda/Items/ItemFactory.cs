using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Sprites;
using MonoZelda.Items.ItemClasses;
using System;

namespace MonoZelda.Items;

public class ItemFactory
{
    private const float FLASHING_TIME = .75f;

    private CollisionController collisionController;

    public ItemFactory( CollisionController collisionController)
    {
        this.collisionController = collisionController;
    }

    public void CreateItem(ItemList itemName, Point spawnPosition)
    {
        var itemDict = new SpriteDict(SpriteType.Items, 0, new Point(0, 0));
        itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemName}");
        IItem item = (IItem)Activator.CreateInstance(itemType);
        item.itemSpawn(itemDict, spawnPosition, collisionController);

    }
}

