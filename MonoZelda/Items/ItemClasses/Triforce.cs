using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Triforce : IItem
{
    private Collidable triforceCollidable;
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

    public Triforce(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict triforceDict, Point spawnPosition, CollisionController collisionController)
    {
        triforceCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 64, 64), graphicsDevice, CollidableType.Item);
        triforceDict.Position = spawnPosition;
        triforceDict.SetSprite("triforce");
        collisionController.AddCollidable(triforceCollidable);
    }

}
