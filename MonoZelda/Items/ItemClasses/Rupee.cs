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
    public Rupee(List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite, List<Item> updateList) : base(roomEnemyList, playerSprite, updateList)
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
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}