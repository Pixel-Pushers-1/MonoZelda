using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class HeartContainer : IItem
{
    private CollidablesManager collidablesManager;
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

    public HeartContainer(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartcontainerDict, Point spawnPosition)
    {
        heartcontainerCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice);
        heartcontainerDict.Position = spawnPosition;
        heartcontainerDict.SetSprite("heartcontainter");
        collidablesManager.AddCollidableObject(heartcontainerCollidable);
    }

}
