﻿using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Key : IItem
{
    private ItemCollidable keyCollidable;
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

    public void itemSpawn(SpriteDict keyDict, Point spawnPosition, CollisionController collisionController)
    {
        keyCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), ItemList.Key);
        collisionController.AddCollidable(keyCollidable);
        keyCollidable.setSpriteDict(keyDict);
        keyDict.Position = spawnPosition;
        keyDict.SetSprite("key_0");
    }

}
