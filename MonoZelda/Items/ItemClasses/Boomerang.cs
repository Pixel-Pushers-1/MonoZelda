using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Boomerang : Item
{
    public Boomerang(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Boomerang;
    }

    public override void ItemSpawn(SpriteDict boomerangDict, Point spawnPosition, CollisionController collisionController)
    {
        PlayerState.HasBoomerang = true;    
        base.ItemSpawn(boomerangDict, spawnPosition, collisionController);
        boomerangDict.SetSprite("boomerang");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
