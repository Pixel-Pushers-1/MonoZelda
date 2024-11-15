using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;

public class Clock : Item
{
    private List<Enemy> roomEnemyList;
    private PlayerCollisionManager playerCollision;
    private const float ENEMY_STUN_TIME = 3f;
    private const int ENEMY_DAMAGE = 0;

    public Clock(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Clock;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("clock");
        roomEnemyList = itemManager.RoomEnemyList;
        playerCollision = itemManager.PlayerCollision;
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        playerCollision.HandleClockCollision();
        foreach(var enemy in roomEnemyList)
        {
            enemy.TakeDamage(ENEMY_STUN_TIME, Direction.None, ENEMY_DAMAGE);
        }
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
