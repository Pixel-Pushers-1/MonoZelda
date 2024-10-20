using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private ICollidable compassCollidable;
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

    public Compass(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition, CollisionController collisionController)
    {
        compassCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60),graphicsDevice);
        collisionController.AddCollidable(compassCollidable);
        compassCollidable.setSpriteDict(compassDict);
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
    }
}
