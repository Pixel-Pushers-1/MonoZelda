using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Heart : IItem
{
    private CollidablesManager collidablesManager;
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

    public Heart(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    { 
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartDict, Point spawnPosition)
    {
        heartCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice);
        heartDict.Position = spawnPosition;
        heartDict.SetSprite("heart_full");
        collidablesManager.AddCollidableObject(heartCollidable);
    }

}