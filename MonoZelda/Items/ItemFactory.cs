﻿using MonoZelda.Controllers;
using MonoZelda.Items.ItemClasses;
using System;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items;

public class ItemFactory
{
    private CollisionController collisionController;
    private ItemManager itemManager;

    public ItemFactory(CollisionController collisionController, ItemManager itemManager)
    {
        this.collisionController = collisionController;
        this.itemManager = itemManager; 
    }

    public void CreateRoomItems()
    {
        List<ItemSpawn> itemSpawnList = itemManager.RoomSpawnList;
        foreach (var itemSpawn in itemSpawnList)
        {
            CreateItem(itemSpawn, false);
        }
    }

    public void CreateItem(ItemSpawn itemSpawn, Boolean dropItem)
    {
        // Get itemClass
        var itemType = Type.GetType($"MonoZelda.Items.ItemClasses.{itemSpawn.ItemType}");
        Item item = (Item)Activator.CreateInstance(itemType,itemManager);

        // Check if item is in spawnList
        if (dropItem == true && itemSpawn.ItemType == ItemList.Key)
        {
            itemManager.AddRoomSpawnItem(itemSpawn);
        }

        // Spawn Item
        item.ItemSpawn(itemSpawn, collisionController);
    }
}

