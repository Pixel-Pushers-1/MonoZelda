using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System.Threading;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Boomerang : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable boomerangCollidable;
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

    public Boomerang(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict boomerangDict, Point spawnPosition)
    {
        boomerangCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice);
        boomerangDict.Position = spawnPosition;
        boomerangDict.SetSprite("boomerang");
        collidablesManager.AddCollidableObject(boomerangCollidable);
    }

}
