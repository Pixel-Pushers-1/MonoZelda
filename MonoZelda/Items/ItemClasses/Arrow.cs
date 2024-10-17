﻿using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Arrow : IItem
{
    private Collidable arrowCollidable;
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

    public Arrow(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict arrowDict, Point spawnPosition, CollisionController collisionController)
    {
        arrowCollidable = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        arrowDict.Position = spawnPosition;
        arrowDict.SetSprite("candle_blue");
        collisionController.AddCollidable(arrowCollidable);
    }
}