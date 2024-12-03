using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public abstract class Item
{
    private const float FLASHING_TIME = 0.75f;
    protected ItemManager itemManager;
    protected SpriteDict itemDict;
    protected ItemCollidable itemCollidable;
    protected ItemSpawn itemSpawn;
    protected ItemList itemType;
    protected Rectangle itemBounds;
    protected bool itemPickedUp;
    SpriteType spriteType;
    public bool ItemPickedUp
    {
        get { return itemPickedUp; }
    }

    public Item(ItemManager itemManager)
    {
        itemPickedUp = false;
        this.itemManager = itemManager;
    }

    public virtual void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        // create item SpriteDict
        if (itemSpawn.ItemType.Equals(ItemList.XPOrb))
        {
            spriteType = SpriteType.Enemies;
        }
        else
        {
            spriteType = SpriteType.Items;
        }
       
        itemDict = new SpriteDict(spriteType, SpriteLayer.Items, itemSpawn.Position);
        itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);

        // create item Collidable 
        itemCollidable = new ItemCollidable(itemBounds, itemType);
        itemCollidable.setItem(this);
        collisionController.AddCollidable(itemCollidable);

        // store itemSpawn for removal
        this.itemSpawn = itemSpawn;
    }

    public virtual void HandleCollision(CollisionController collisionController)
    {
        // unregister collidable and remove from collisionController
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // unregister spriteDict
        itemDict.Unregister();

        // update pickUp boolean
        itemManager.RemoveRoomSpawnItem(itemSpawn);
        itemPickedUp = true;
    }

    public virtual void Update()
    {
        // Empty
    }

}

