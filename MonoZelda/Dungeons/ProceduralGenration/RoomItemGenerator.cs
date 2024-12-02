using MonoZelda.Enemies;
using MonoZelda.Items;
using System.Collections.Generic;
using System;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomItemGenerator
{
    private const int UTILITY_ITEM_POOL_LENGTH = 2;
    private const int HEALTH_ITEM_POOL_LENGTH = 5;
    private static readonly ItemList[] UtilityItemPool = { ItemList.Bomb, ItemList.Rupee };
    private static readonly ItemList[] HealthItemPool= { ItemList.BluePotion, ItemList.RedPotion, ItemList.HeartContainer, ItemList.Fairy, ItemList.Heart };


    public RoomItemGenerator()
    {

    }

    public List<ItemList> GenerateItemsForRooms(int roomNumber, int playerLevel, int playerHealth)
    {
        List<ItemList> items = new List<ItemList>();
        Random random = new Random();

        return items;
    }

}
