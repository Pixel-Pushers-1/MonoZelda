using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision;

namespace MonoZelda.Items.ItemClasses;

public abstract class Item
{
    protected ItemCollidable itemCollidable;
    protected ItemList itemType;
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

    public virtual void ItemSpawn(SpriteDict itemDict, Point  spawnPosition, CollisionController collisionController)
    {
        itemCollidable = new ItemCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), itemType);
        itemCollidable.setItem(this);
        collisionController.AddCollidable(itemCollidable);
        itemCollidable.setSpriteDict(itemDict);
        itemDict.Position = spawnPosition;
    }

    public virtual void HandleCollision(SpriteDict itemCollidableDict,CollisionController collisionController)
    {
        itemCollidableDict.Unregister();
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);
    }

}

