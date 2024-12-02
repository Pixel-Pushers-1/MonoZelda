using MonoZelda.Items;

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

}
