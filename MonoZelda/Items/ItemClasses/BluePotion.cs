﻿using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : IItem
{
    private ItemCollidable bluepotionCollidable;
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

    public void itemSpawn(SpriteDict bluepotionDict, Point spawnPosition, CollisionController collisionController)
    {
        bluepotionCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), ItemList.BluePotion);
        collisionController.AddCollidable(bluepotionCollidable);
        bluepotionCollidable.setSpriteDict(bluepotionDict);
        bluepotionDict.Position = spawnPosition;
        bluepotionDict.SetSprite("potion_blue");
    }
}