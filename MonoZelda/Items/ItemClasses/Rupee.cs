using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : Item
{
    public Rupee(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Rupee;
    }

    public override void ItemSpawn(SpriteDict rupeeDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(rupeeDict,spawnPosition,collisionController);
        rupeeDict.SetSprite("rupee");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        PlayerState.AddRupees(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}