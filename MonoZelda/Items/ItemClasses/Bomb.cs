using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Link;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : Item
{
    public Bomb(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Bomb;
    }

    public override void ItemSpawn(Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(spawnPosition, collisionController);  
        itemDict.SetSprite("bomb");   
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddBombs(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}