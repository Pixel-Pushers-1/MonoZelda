using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System.Threading;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Map : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable mapCollidable;
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

    public Map(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict mapDict, Point spawnPosition)
    {
        mapCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice);
        mapDict.Position = spawnPosition;
        mapDict.SetSprite("map");
        collidablesManager.AddCollidableObject(mapCollidable);
    }
}

