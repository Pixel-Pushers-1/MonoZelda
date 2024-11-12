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
    private List<Enemy> roomEnemyList;
    private PlayerCollisionManager playerCollision;
    private List<Item> updateList;

    public ItemFactory(CollisionController collisionController, List<ItemSpawn> itemSpawnList, List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision)
    {
        this.collisionController = collisionController;
        this.itemSpawnList = itemSpawnList; 
        this.roomEnemyList = roomEnemyList;
        this.playerCollision = playerCollision;
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
        Item item = (Item)Activator.CreateInstance(itemType,roomEnemyList,playerCollision,updateList);
        item.ItemSpawn(itemDict, spawnPosition, collisionController);
    }

    public void Update()
    {
        for(int i = 0; i < updateList.Count; i++)
        {
            updateList[i].Update();
        }
    }
}

