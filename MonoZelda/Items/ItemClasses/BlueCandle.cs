using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class BlueCandle : IItem
{
    private Collidable bluecandleCollidable;
    private bool itemPickedUp;

    public bool ItemPickedUp
    {
        get
        {
            return itemPickedUp;
        }
        set
        {
            itemPickedUp = value;
        }
    }

    public BlueCandle()
    {
    }

    public void itemSpawn(SpriteDict bluecandleDict, Point spawnPosition, CollisionController collisionController)
    {
        bluecandleCollidable = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 28, 60), CollidableType.Item);
        collisionController.AddCollidable(bluecandleCollidable);
        bluecandleCollidable.setSpriteDict(bluecandleDict);
        bluecandleDict.Position = spawnPosition;
        bluecandleDict.SetSprite("candle_blue");    }
}

