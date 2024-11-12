using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Compass : Item
{
    public Compass(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Compass;
    }

    public override void ItemSpawn(SpriteDict compassDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(compassDict, spawnPosition, collisionController);
        compassDict.SetSprite("compass");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
