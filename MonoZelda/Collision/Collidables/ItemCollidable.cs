using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Items;
using System.Collections.Generic;
using System;
using MonoZelda.Sound;

namespace MonoZelda.Collision;

public class ItemCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public ItemList itemType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }

    private readonly CollisionHitboxDraw hitbox;

    private readonly Dictionary<ItemList, Action> itemSoundEffects = new()
    {
       { ItemList.Heart, () => SoundManager.PlaySound("LOZ_Get_Heart" ,false) },
       { ItemList.Rupee, () => SoundManager.PlaySound("LOZ_Get_Rupee",false) },
    };

    public ItemCollidable(Rectangle bounds, GraphicsDevice graphicsDevice, ItemList itemType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        type = CollidableType.Item;
        this.itemType = itemType;
    }

    public void PlaySound()
    {
        if (itemSoundEffects.ContainsKey(itemType))
        {
            itemSoundEffects[itemType].Invoke();
        }
        else
        {
            SoundManager.PlaySound("LOZ_Get_Item", false);
        }
    }

    public void UnregisterHitbox()
    {
        hitbox.Unregister();
    }

    public bool Intersects(ICollidable other)
    {
        return Bounds.Intersects(other.Bounds);
    }

    public Rectangle GetIntersectionArea(ICollidable other)
    {
        return Rectangle.Intersect(Bounds, other.Bounds);
    }

    public override string ToString()
    {
        return $"{type}";
    }

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }
}
