using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class HeartContainer : Item
{
    public HeartContainer()
    {
        itemType = ItemList.HeartContainer;
    }

    public override void ItemSpawn(SpriteDict heartcontainerDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(heartcontainerDict, spawnPosition, collisionController); 
        heartcontainerDict.SetSprite("heartcontainer");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }

}
