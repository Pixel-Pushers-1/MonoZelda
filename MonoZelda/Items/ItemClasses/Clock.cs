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
    public Clock(List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite, List<Item> updateList) : base(roomEnemyList, playerSprite, updateList)
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
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
