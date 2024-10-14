using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class Fairy : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable fairyCollidable;
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

    public Fairy(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict fairyDict, Point spawnPosition)
    {
        fairyCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice);
        fairyDict.Position = spawnPosition;
        fairyDict.SetSprite("fairy");
        collidablesManager.AddCollidableObject(fairyCollidable);
    }

}
