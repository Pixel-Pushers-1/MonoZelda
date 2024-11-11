using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : Item
{
    public Rupee()
    {
        itemType = ItemList.Rupee;
    }

    public override void ItemSpawn(SpriteDict rupeeDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(rupeeDict,spawnPosition,collisionController);
        rupeeDict.SetSprite("rupee");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}