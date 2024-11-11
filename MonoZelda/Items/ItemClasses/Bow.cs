using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;
public class Bow : Item
{
    public Bow()
    {
        itemType = ItemList.Bow;
    }

    public override void ItemSpawn(SpriteDict bowDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bowDict, spawnPosition, collisionController);
        bowDict.SetSprite("bow");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}