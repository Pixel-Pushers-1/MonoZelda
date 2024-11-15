using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
using MonoZelda.Dungeons;
namespace MonoZelda.Items.ItemClasses;

public class Boomerang : Item
{
    public Boomerang(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Boomerang;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        PlayerState.HasBoomerang = true;    
        base.ItemSpawn(itemSpawn, collisionController);
        itemDict.SetSprite("boomerang");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.HasBoomerang = true;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
