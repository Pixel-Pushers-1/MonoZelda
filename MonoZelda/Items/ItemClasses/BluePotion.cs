using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : IItem
{
    private Collidable bluepotionCollidable;
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
        bluepotionCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(bluepotionCollidable);
        bluepotionCollidable.setSpriteDict(bluepotionDict);
        bluepotionDict.Position = spawnPosition;
        bluepotionDict.SetSprite("potion_blue");
    }
}