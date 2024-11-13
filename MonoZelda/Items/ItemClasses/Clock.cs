using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Clock : Item
{
    public Clock(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Clock;
    }

    public override void ItemSpawn(SpriteDict clockDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(clockDict, spawnPosition, collisionController);
        clockDict.SetSprite("clock");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        playerCollision.HandleClockCollision();
        foreach(var enemy in roomEnemyList)
        {
            enemy.TakeDamage(3f, Direction.None, 0);
        }
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
