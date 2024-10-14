using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision.Collidables;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public class BluePotion : IItem
{
    private CollidablesManager collidablesManager;
    private Collidable bluepotionCollidable;
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

    public BluePotion(CollidablesManager collidablesManager, GraphicsDevice graphicsDevice)
    {
        this.collidablesManager = collidablesManager;
        this.graphicsDevice = graphicsDevice;   
    }

    public void itemSpawn(SpriteDict bluepotionDict, Point spawnPosition)
    {
        bluepotionCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice);
        bluepotionDict.Position = spawnPosition;
        bluepotionDict.SetSprite("potion_blue");
        collidablesManager.AddCollidableObject(bluepotionCollidable);
    }
}