using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Map : IItem
{
    private ICollidable mapCollidable;
    private bool itemPickedUp;
    private GraphicsDevice graphicsDevice;

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

    public Map(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict mapDict, Point spawnPosition, CollisionController collisionController)
    {
        mapCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice);
        collisionController.AddCollidable(mapCollidable);
        mapCollidable.setSpriteDict(mapDict);
        mapDict.Position = spawnPosition;
        mapDict.SetSprite("map");
    }
}

