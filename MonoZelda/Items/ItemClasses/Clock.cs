﻿using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Clock : IItem
{
    private CollisionController collisionController;
    private Collidable clockCollidable;
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

    public Clock(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;   
    }

    public void itemSpawn(SpriteDict clockDict, Point spawnPosition)
    {
        clockCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice, "Clock");
        clockDict.Position = spawnPosition;
        clockDict.SetSprite("clock");
        collisionController.AddCollidable(clockCollidable);
    }

}
