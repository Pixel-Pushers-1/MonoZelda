using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;
namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Heart : IItem
{
    private CollisionController collisionController;
    private Collidable heartCollidable;
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

    public Heart(CollisionController collisionController, GraphicsDevice graphicsDevice)
    { 
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartDict, Point spawnPosition)
    {
        heartCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice, "Heart");
        heartDict.Position = spawnPosition;
        heartDict.SetSprite("heart_full");
        collisionController.AddCollidable(heartCollidable);
    }

}