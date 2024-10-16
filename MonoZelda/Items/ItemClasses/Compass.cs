﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private CollisionController collisionController;
    private Collidable compassCollidable;
    private GraphicsDevice graphicsDevice;
    private bool itemPickedUp;

    public bool ItemPickedUp
    {
        get
        {
            return itemPickedUp;
        }
        set
        {
            itemPickedUp = value;
        }
    }

    public Compass(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition)
    {
        compassCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice, "Compass");
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
        collisionController.AddCollidable(compassCollidable);
    }
}
