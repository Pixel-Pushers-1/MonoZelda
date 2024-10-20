using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : IItem
{
    private ICollidable bluepotionCollidable;
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

    public BluePotion(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict bluepotionDict, Point spawnPosition, CollisionController collisionController)
    {
        bluepotionCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice);
        collisionController.AddCollidable(bluepotionCollidable);
        bluepotionCollidable.setSpriteDict(bluepotionDict);
        bluepotionDict.Position = spawnPosition;
        bluepotionDict.SetSprite("potion_blue");
    }
}