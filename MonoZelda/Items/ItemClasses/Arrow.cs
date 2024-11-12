using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Arrow : Item
{
    public Arrow(List<IEnemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList,playerCollision,updateList)
    {
        itemType = ItemList.Arrow;
    }

    public override void ItemSpawn(SpriteDict arrowDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(arrowDict, spawnPosition, collisionController);
        arrowDict.SetSprite("arrow");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}