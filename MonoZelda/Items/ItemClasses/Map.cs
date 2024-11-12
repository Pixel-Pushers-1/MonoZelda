using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.UI;

namespace MonoZelda.Items.ItemClasses;

public class Map : Item
{
    public Map(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
    {
        itemType = ItemList.Map;
    }
    public override void ItemSpawn(SpriteDict mapDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(mapDict, spawnPosition, collisionController);        
        mapDict.SetSprite("map");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        HUDMapWidget.SetMapVisible(true);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}

