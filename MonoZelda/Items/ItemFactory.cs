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
    private GraphicsDevice graphicsDevice;
    private CollisionController collisionController;
    private ContentManager contentManager;

    public ItemFactory(GraphicsDevice graphicsDevice, CollisionController collisionController, ContentManager contentManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.collisionController = collisionController;
        this.contentManager = contentManager;  
    }

    public void CreateItem(ItemList itemName, Point spawnPosition)
    {
        var itemDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, new Point(0, 0));
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemName}");
        IItem item = (IItem)Activator.CreateInstance(itemType, graphicsDevice);
        item.itemSpawn(itemDict, spawnPosition, collisionController);

    }
}

