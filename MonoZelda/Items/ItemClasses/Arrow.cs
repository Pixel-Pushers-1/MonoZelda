using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Arrow : IItem
{
    private CollidablesManager collidablesManager;
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

    public Arrow(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict arrowDict, Point spawnPosition)
    {
        arrowCollidable = new Collidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), graphicsDevice);
        arrowDict.Position = spawnPosition;
        arrowDict.SetSprite("candle_blue");
        collidablesManager.AddCollidableObject(arrowCollidable);
    }
}