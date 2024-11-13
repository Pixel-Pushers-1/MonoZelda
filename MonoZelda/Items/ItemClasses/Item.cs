using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision;

namespace MonoZelda.Items.ItemClasses;

public abstract class Item
{
    private const float FLASHING_TIME = 0.75f;
    protected ItemManager itemManager;
    protected SpriteDict itemDict;
    protected ItemCollidable itemCollidable;
    protected ItemList itemType;
    protected Rectangle itemBounds;
    protected bool itemPickedUp;

    public bool ItemPickedUp
    {
        get { return itemPickedUp; }
    }

    public Item(ItemManager itemManager)
    {
        itemPickedUp = false;
        this.itemManager = itemManager;
    }

    public virtual void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        // create item SpriteDict
        itemDict = new SpriteDict(SpriteType.Items, SpriteLayer.Items, spawnPosition);
        itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);

        // create item Collidable 
        itemCollidable = new ItemCollidable(itemBounds, itemType);
        collisionController.AddCollidable(itemCollidable);

    }

    public virtual void HandleCollision(CollisionController collisionController)
    {
        // unregister collidable and remove from collisionController
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // unregister spriteDict
        itemDict.Unregister();

        // update pickUp boolean
        itemPickedUp = true;
    }

    public virtual void Update()
    {
        // Empty
    }

}

