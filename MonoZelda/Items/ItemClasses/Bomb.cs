using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : Item
{
    public Bomb(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Bomb;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        base.ItemSpawn(itemSpawn, collisionController);  
        itemDict.SetSprite("bomb");   
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddBombs(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}