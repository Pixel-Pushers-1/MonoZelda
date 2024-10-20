using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class HeartContainer : IItem
{
    private ICollidable heartcontainerCollidable;
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

    public HeartContainer(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartcontainerDict, Point spawnPosition, CollisionController collisionController)
    {
        heartcontainerCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), graphicsDevice);
        collisionController.AddCollidable(heartcontainerCollidable);
        heartcontainerCollidable.setSpriteDict(heartcontainerDict);
        heartcontainerDict.Position = spawnPosition;
        heartcontainerDict.SetSprite("heartcontainer");
    }

}
