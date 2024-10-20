using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : IItem
{
    private ICollidable fairyCollidable;
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

    public Fairy(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict fairyDict, Point spawnPosition, CollisionController collisionController)
    {
        fairyCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice);
        collisionController.AddCollidable(fairyCollidable);
        fairyCollidable.setSpriteDict(fairyDict);
        fairyDict.Position = spawnPosition;
        fairyDict.SetSprite("fairy");
    }

}
