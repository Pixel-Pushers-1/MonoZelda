using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;
public class Bow : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable bowCollidable;
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

    public Bow(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict bowDict, Point spawnPosition)
    {
        bowCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice);
        bowDict.Position = spawnPosition;
        bowDict.SetSprite("bow");
        collidablesManager.AddCollidableObject(bowCollidable);
    }

}