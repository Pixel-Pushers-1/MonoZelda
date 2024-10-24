﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private Collidable compassCollidable;
    private bool itemPickedUp;
    private GraphicsDevice graphicsDevice;

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

    public Compass(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition, CollisionController collisionController)
    {
        compassCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(compassCollidable);
        compassCollidable.setSpriteDict(compassDict);
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
    }
}
