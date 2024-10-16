using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System.Threading;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Key : IItem
{
    private CollisionController collisionController;
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

    public Key(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict keyDict, Point spawnPosition)
    {
        keyCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, "Key");
        keyDict.Position = spawnPosition;
        keyDict.SetSprite("key_0");
        collisionController.AddCollidable(keyCollidable);
    }

}
