using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Clock : IItem
{
    private Collidable clockCollidable;
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

    public Clock(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;   
    }

    public void itemSpawn(SpriteDict clockDict, Point spawnPosition, CollisionController collisionController)
    {
        clockCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(clockCollidable);
        clockCollidable.setSpriteDict(clockDict);
        clockDict.Position = spawnPosition;
        clockDict.SetSprite("clock");
    }

}
