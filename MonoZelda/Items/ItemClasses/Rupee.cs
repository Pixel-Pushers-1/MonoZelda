using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : IItem
{
    private ICollidable rupeeCollidable;
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

    public Rupee(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict rupeeDict, Point spawnPosition, CollisionController collisionController)
    {
        rupeeCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice);
        collisionController.AddCollidable(rupeeCollidable);
        rupeeCollidable.setSpriteDict(rupeeDict);
        rupeeDict.Position = spawnPosition;
        rupeeDict.SetSprite("rupee");
    }

}