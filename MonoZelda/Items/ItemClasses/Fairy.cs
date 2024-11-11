using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : Item
{
    public Fairy(List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite, List<Item> updateList) : base(roomEnemyList, playerSprite, updateList)
    {
        itemType = ItemList.Fairy;
    }

    public override void ItemSpawn(SpriteDict fairyDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(fairyDict, spawnPosition, collisionController);
        fairyDict.SetSprite("fairy");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
