using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System.Threading;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Key : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable keyCollidable;
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

    public Key(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict keyDict, Point spawnPosition)
    {
        keyCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice);
        keyDict.Position = spawnPosition;
        keyDict.SetSprite("key_0");
        collidablesManager.AddCollidableObject(keyCollidable);
    }

}
