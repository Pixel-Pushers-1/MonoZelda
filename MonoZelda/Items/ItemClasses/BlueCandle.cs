using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class BlueCandle : IItem
{
    private ItemCollidable bluecandleCollidable;
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

    public void itemSpawn(SpriteDict bluecandleDict, Point spawnPosition, CollisionController collisionController)
    {
        bluecandleCollidable = new ItemCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 28, 60), ItemList.BlueCandle);
        collisionController.AddCollidable(bluecandleCollidable);
        bluecandleCollidable.setSpriteDict(bluecandleDict);
        bluecandleDict.Position = spawnPosition;
        bluecandleDict.SetSprite("candle_blue");    }
}

