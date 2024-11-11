using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Clock : Item
{
    public Clock()
    {
        itemType = ItemList.Clock;
    }

    public override void ItemSpawn(SpriteDict clockDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(clockDict, spawnPosition, collisionController);
        clockDict.SetSprite("clock");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
