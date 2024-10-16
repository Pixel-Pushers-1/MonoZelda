using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class BlueCandle : IItem
{
    private CollisionController collisionController;
    private Collidable bluecandleCollidable;
    private GraphicsDevice graphicsDevice;
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

    public BlueCandle(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict bluecandleDict, Point spawnPosition)
    {
        bluecandleCollidable = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), graphicsDevice, "BlueCandle");
        bluecandleDict.Position = spawnPosition;
        bluecandleDict.SetSprite("candle_blue");
        collisionController.AddCollidable(bluecandleCollidable);
    }
}

