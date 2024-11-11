using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : Item
{
    public Bomb(List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite, List<Item> updateList) : base(roomEnemyList, playerSprite, updateList)
    {
        itemType = ItemList.Bomb;
    }

    public override void ItemSpawn(SpriteDict bombDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bombDict, spawnPosition, collisionController);  
        bombDict.SetSprite("bomb");   
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}