using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision;
using System.Collections.Generic;
using MonoZelda.Enemies;
using MonoZelda.Link;

namespace MonoZelda.Items.ItemClasses;

public abstract class Item
{
    protected PlayerCollisionManager playerCollision;
    protected ItemCollidable itemCollidable;
    protected List<IEnemy> roomEnemyList;
    protected List<Item> updateList;
    protected ItemList itemType;

    public Item(List<IEnemy> roomEnemyList, PlayerCollisionManager playerCollision,List<Item> updateList)
    {
        this.roomEnemyList = roomEnemyList;
        this.updateList = updateList;
        this.playerCollision = playerCollision;
    }

    public virtual void ItemSpawn(SpriteDict itemDict, Point  spawnPosition, CollisionController collisionController)
    {
        itemCollidable = new ItemCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 60, 60), itemType);
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

    public virtual void Update()
    {
        // Empty
    }

}

