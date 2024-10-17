using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : IItem
{
    private CollisionController collisionController;
    private Collidable rupeeCollidable;
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

    public Rupee(GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict rupeeDict, Point spawnPosition, CollisionController collisionController)
    {
        rupeeCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        rupeeDict.Position = spawnPosition;
        rupeeDict.SetSprite("rupee");
        collisionController.AddCollidable(rupeeCollidable);
    }

}