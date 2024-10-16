using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Arrow : IItem
{
    private CollisionController collisionController;
    private Collidable arrowCollidable;
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

    public Arrow(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict arrowDict, Point spawnPosition)
    {
        arrowCollidable = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), graphicsDevice, "Arrow");
        arrowDict.Position = spawnPosition;
        arrowDict.SetSprite("candle_blue");
        collisionController.AddCollidable(arrowCollidable);
    }
}