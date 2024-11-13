﻿using MonoZelda.Dungeons;
using MonoZelda.Enemies;
using MonoZelda.Items.ItemClasses;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items;

public class ItemManager
{
    private List<ItemSpawn> roomSpawnList;
    private List<Enemy> roomEnemyList;
    private PlayerCollisionManager playerCollision;
    private List<Item> itemUpdateList;

    public ItemManager(List<ItemSpawn> roomSpawnList, List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision)
    {
        this.roomSpawnList = roomSpawnList;
        this.roomEnemyList = roomEnemyList;
        this.playerCollision = playerCollision;   
        itemUpdateList = new List<Item>();
    }

    public List<ItemSpawn> RoomSpawnList
    {
        get { return roomSpawnList; }
    }

    public List<Enemy> RoomEnemyList
    {
        get { return roomEnemyList; }
    }

    public PlayerCollisionManager PlayerCollision
    {
        get { return playerCollision; }
    }

    public void AddUpdateItem(Item item)
    {
        itemUpdateList.Add(item);
    }

    public void RemoveUpdateItem(Item item)
    {
        itemUpdateList.Remove(item);
    }

    public void Update()
    {
        foreach(var item in itemUpdateList)
        {
            item.Update();
        }
    }
}

