using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class HeartContainer : Item
{
    public HeartContainer(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.HeartContainer;
    }

    public override void ItemSpawn(SpriteDict heartcontainerDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(heartcontainerDict, spawnPosition, collisionController); 
        heartcontainerDict.SetSprite("heartcontainer");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        PlayerState.MaxHealth += 2;
        PlayerState.Health += 2;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }

}
