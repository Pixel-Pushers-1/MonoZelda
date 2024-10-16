using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Scenes;
using MonoZelda.Collision;
using MonoZelda.Items.ItemClasses;
using System;
using MonoZelda.Controllers;

namespace MonoZelda.Items;

public class ItemFactory
{
    private GraphicsDevice graphicsDevice;
    private CollisionController collisionController;

    public ItemFactory(GraphicsDevice graphicsDevice, CollisionController collisionController)
    {
        this.graphicsDevice = graphicsDevice;
        this.collisionController = collisionController;
    }

    public T CreateItem<T>() where T : IItem
    {
        // Create an instance of the item, passing in the common objects to the constructor
        return (T)Activator.CreateInstance(typeof(T), collisionController, graphicsDevice);
    }
}

