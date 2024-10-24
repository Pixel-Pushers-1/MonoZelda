﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Content;
using MonoZelda.Sprites;
using MonoZelda.Items.ItemClasses;
using System;

namespace MonoZelda.Items;

public class ItemFactory
{ 
    private CollisionController collisionController;
    private ContentManager contentManager;
    private GraphicsDevice graphicsDevice;

    public ItemFactory( CollisionController collisionController, ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.contentManager = contentManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void CreateItem(ItemList itemName, Point spawnPosition)
    {
        var itemDict = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, new Point(0, 0));
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemName}");
        IItem item = (IItem)Activator.CreateInstance(itemType,graphicsDevice);
        item.itemSpawn(itemDict, spawnPosition, collisionController);

    }
}

