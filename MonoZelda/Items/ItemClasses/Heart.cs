using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Heart : Item
{
    public Heart(List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite, List<Item> updateList) : base(roomEnemyList, playerSprite, updateList)
    {
        itemType = ItemList.Heart;
    }

    public override void ItemSpawn(SpriteDict heartDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(heartDict, spawnPosition, collisionController);  
        heartDict.SetSprite("heart_full");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Heart", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}