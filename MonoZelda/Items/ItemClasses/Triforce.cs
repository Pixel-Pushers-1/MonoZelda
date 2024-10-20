using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : IItem
{
    private ICollidable triforceCollidable;
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

    public Triforce(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict triforceDict, Point spawnPosition, CollisionController collisionController)
    {
        triforceCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), graphicsDevice);
        collisionController.AddCollidable(triforceCollidable);
        triforceCollidable.setSpriteDict(triforceDict);
        triforceDict.Position = spawnPosition;
        triforceDict.SetSprite("triforce");
    }

}
