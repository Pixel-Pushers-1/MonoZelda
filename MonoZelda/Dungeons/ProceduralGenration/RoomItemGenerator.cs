using MonoZelda.Enemies;
using MonoZelda.Items;
using System.Collections.Generic;
using System;
using MonoZelda.Link;
using System.ComponentModel;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomItemGenerator
{
    // constants
    private const int UTILITY_ITEM_POOL_LENGTH = 2;
    private const int HEALTH_ITEM_POOL_LENGTH = 4;
    private const int PLAYER_MAX_HEALTH = 12;
    private const int ROOM_HEART_CONTAINER_SPAWN_COUNT = 5;
    private static readonly ItemList[] UtilityItemPool = { ItemList.Bomb, ItemList.Rupee };
    private static readonly ItemList[] HealthItemPool= { ItemList.BluePotion, ItemList.RedPotion, ItemList.Fairy };

    // variables
    private double bluePotionRate;
    private double redPotionRate;
    private double fairyRate;
    private Random random;

    public RoomItemGenerator()
    {
        bluePotionRate = 0.05f;
        redPotionRate = 0.02f;
        fairyRate = 0.10f;
        random = new Random();
    }

    public List<ItemList> GenerateItemsForRoom(int roomNumber, int playerLevel, int playerHealth)
    {
        List<ItemList> items = new List<ItemList>();

        // adjust health item spawn rate based on player health, roomNumber, and player level
        AdjustSpawnRates(roomNumber, playerLevel, playerHealth);

        // add Utility items
        AddUtilityItems(items);

        // add health items
        AddHealthItems(items);

        // add heart container
        AddHeartContainer(roomNumber, items);

        return items;
    }

    private void AdjustSpawnRates(int roomNumber, int playerHealth, int playerLevel)
    {
        // adjust according to health
        double healthModifier = (double)(PLAYER_MAX_HEALTH - playerHealth) / PLAYER_MAX_HEALTH;
        redPotionRate += healthModifier * 0.1;
        bluePotionRate += healthModifier * 0.2;
        fairyRate += healthModifier * 0.15;

        // adjust according to level and roomNumber
        double difficultyModifier = (roomNumber + playerLevel) * 0.01;
        redPotionRate = Math.Max(0, redPotionRate - difficultyModifier);
        bluePotionRate = Math.Max(0, bluePotionRate - difficultyModifier);
        fairyRate = Math.Max(0, fairyRate - difficultyModifier);
    }

    private void AddHealthItems(List<ItemList> items)
    {
        double roll = random.NextDouble();
        if (roll < redPotionRate)
        {
            items.Add(ItemList.RedPotion);
        }
        else if (roll < redPotionRate + bluePotionRate)
        {
            items.Add(ItemList.BluePotion);
        }
        else if (roll < redPotionRate + bluePotionRate + fairyRate)
        {
            items.Add(ItemList.Fairy);
        }
    }

    private void AddUtilityItems(List<ItemList> items)
    {
        double rate = 0.05;
        double roll = random.NextDouble();
        if(roll <= rate)
        {
            items.Add(ItemList.Bomb);
            items.Add(ItemList.Rupee);
        }
    }

    private void AddHeartContainer(int roomNumber, List<ItemList> items)
    {
        if((PlayerState.MaxHealth != PLAYER_MAX_HEALTH) && (roomNumber % ROOM_HEART_CONTAINER_SPAWN_COUNT == 0))
        {
            items.Add(ItemList.HeartContainer);
        }
    }

}
