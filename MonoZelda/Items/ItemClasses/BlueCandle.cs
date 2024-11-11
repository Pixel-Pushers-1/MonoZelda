using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class BlueCandle : Item
{
    public BlueCandle()
    {
        itemType = ItemList.BlueCandle;
    }

    public override void ItemSpawn(SpriteDict bluecandleDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bluecandleDict, spawnPosition, collisionController);
        bluecandleDict.SetSprite("candle_blue");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}

