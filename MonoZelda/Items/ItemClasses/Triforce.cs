using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : Item
{
    public Triforce(List<IEnemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Triforce;
    }

    public override void ItemSpawn(SpriteDict triforceDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(triforceDict, spawnPosition, collisionController);   
        triforceDict.SetSprite("triforce");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
