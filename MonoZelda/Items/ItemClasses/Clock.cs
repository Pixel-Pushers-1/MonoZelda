using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Clock : Item
{
    private List<Enemy> roomEnemyList;
    private PlayerCollisionManager playerCollision;

    public Clock(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Clock;
    }

    public override void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(spawnPosition, collisionController);
        itemDict.SetSprite("clock");
        roomEnemyList = itemManager.RoomEnemyList;
        playerCollision = itemManager.PlayerCollision;
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        playerCollision.HandleClockCollision();
        foreach(var enemy in roomEnemyList)
        {
            enemy.TakeDamage(3f, Direction.None, 0);
        }
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
