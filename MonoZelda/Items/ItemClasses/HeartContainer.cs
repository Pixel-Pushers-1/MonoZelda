using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class HeartContainer : IItem
{
    private CollisionController collisionController;
    private Collidable heartcontainerCollidable;
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

    public HeartContainer(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartcontainerDict, Point spawnPosition)
    {
        heartcontainerCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice, "HeartContainer");
        heartcontainerDict.Position = spawnPosition;
        heartcontainerDict.SetSprite("heartcontainter");
        collisionController.AddCollidable(heartcontainerCollidable);
    }

}
