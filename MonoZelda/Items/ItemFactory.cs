using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Scenes;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Items.ItemClasses;
using System;
using System.Collections.Generic;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items;

public class ItemFactory
{
    private GraphicsDevice graphicsDevice;
    private CollisionController collisionController;

    public ItemFactory(GraphicsDevice graphicsDevice, CollisionController collisionController)
    {
        this.graphicsDevice = graphicsDevice;
        this.collisionController = collisionController;
    }

    public IItem CreateItem(ItemType itemType)
    {
        // Build the full class name using the enum value
        string className = $"PixelPushers.MonoZelda.Items.{itemType}";

        // Get the Type of the class from its name
        Type itemTypeClass = Type.GetType(className);

        if (itemTypeClass != null)
        {
            // Use Activator.CreateInstance to create the item, passing the necessary constructor arguments
            return (IItem)Activator.CreateInstance(itemTypeClass, graphicsDevice, collisionController);
        }

        throw new InvalidOperationException($"Item type {itemType} not found.");
    }
}

