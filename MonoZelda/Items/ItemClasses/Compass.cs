using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Sprites;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable compassCollidable;
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

    public Compass(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition)
    {
        compassCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice);
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
        collidablesManager.AddCollidableObject(compassCollidable);
    }
}
