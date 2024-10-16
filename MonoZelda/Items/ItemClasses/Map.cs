using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System.Threading;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Map : IItem
{
    private CollisionController collisionController;
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

    public Map(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict mapDict, Point spawnPosition)
    {
        mapCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, "Map");
        mapDict.Position = spawnPosition;
        mapDict.SetSprite("map");
        collisionController.AddCollidable(mapCollidable);
    }
}

