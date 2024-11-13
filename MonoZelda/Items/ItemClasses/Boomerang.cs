using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;
namespace MonoZelda.Items.ItemClasses;

public class Boomerang : Item
{
    public Boomerang(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Boomerang;
    }

    public override void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        PlayerState.HasBoomerang = true;    
        base.ItemSpawn(spawnPosition, collisionController);
        itemDict.SetSprite("boomerang");
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.HasBoomerang = true;
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
