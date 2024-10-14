using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Scenes;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Items.ItemClasses;
using System;
using System.Collections.Generic;

namespace PixelPushers.MonoZelda.Items;

public class ItemFactory
{
    private GraphicsDevice graphicsDevice;
    private CollidablesManager collidablesManager;

    public ItemFactory(GraphicsDevice graphicsDevice, CollidablesManager collidablesManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.collidablesManager = collidablesManager;
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
            return (IItem)Activator.CreateInstance(itemTypeClass, graphicsDevice, collidablesManager);
        }

        throw new InvalidOperationException($"Item type {itemType} not found.");
    }
}

