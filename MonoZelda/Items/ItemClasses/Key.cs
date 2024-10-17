using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Key : IItem
{
    private Collidable keyCollidable;
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

    public Key(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict keyDict, Point spawnPosition, CollisionController collisionController)
    {
        keyCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        keyDict.Position = spawnPosition;
        keyDict.SetSprite("key_0");
        collisionController.AddCollidable(keyCollidable);
    }

}
