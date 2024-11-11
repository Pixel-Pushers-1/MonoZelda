using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Boomerang : Item
{
    public Boomerang()
    {
        itemType = ItemList.Boomerang;
    }

    public override void ItemSpawn(SpriteDict boomerangDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(boomerangDict, spawnPosition, collisionController);
        boomerangDict.SetSprite("boomerang");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
