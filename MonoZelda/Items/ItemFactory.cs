using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sprites;
using MonoZelda.Items.ItemClasses;
using System;
using System.Collections.Generic;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Enemies;

namespace MonoZelda.Items;

public class ItemFactory
{ 
    private CollisionController collisionController;
    private List<ItemSpawn> itemSpawnList;
    private List<IEnemy> roomEnemyList;
    private PlayerSpriteManager playerSprite;
    private List<Item> updateList;

    public ItemFactory(CollisionController collisionController, List<ItemSpawn> itemSpawnList, List<IEnemy> roomEnemyList, PlayerSpriteManager playerSprite)
    {
        this.collisionController = collisionController;
        this.itemSpawnList = itemSpawnList; 
        this.roomEnemyList = roomEnemyList;
        this.playerSprite = playerSprite;
        updateList = new List<Item>();
    }

    public void CreateRoomItems()
    {
        foreach (var itemSpawn in itemSpawnList)
        {
            CreateItem(itemSpawn.ItemType,itemSpawn.Position);
        }
    }

    public void CreateItem(ItemList itemName, Point spawnPosition)
    {
        var itemDict = new SpriteDict(SpriteType.Items, 0, new Point(0, 0));
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemName}");
        Item item = (Item)Activator.CreateInstance(itemType,roomEnemyList,playerSprite,updateList);
        item.ItemSpawn(itemDict, spawnPosition, collisionController);
    }
}

