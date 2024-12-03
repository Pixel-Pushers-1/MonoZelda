using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
using MonoZelda.Collision;
using System;
using MonoZelda.Events;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : Item
{
    private float FLASHING_TIME = 0.75f;
    private PlayerCollisionManager playerCollision;
    private event Action LevelComplete;

    public Triforce(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Triforce;
        playerCollision = itemManager.PlayerCollision;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        Point offset = new Point(32, 12);
        itemBounds = new Rectangle(itemSpawn.Position + offset, new Point(56, 56));
        itemSpawn.Position += offset;
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("triforce");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        // unregister collidable and remove from collisionController
        itemDict.Unregister();
        itemCollidable.UnregisterHitbox();
        collisionController.RemoveCollidable(itemCollidable);

        // remove item from roomSpawn list
        itemManager.RemoveRoomSpawnItem(itemSpawn);

        // call the end game envent
        EventManager.TriggerLevelCompletionAnimation();
    }
}
