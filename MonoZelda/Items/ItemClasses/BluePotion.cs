using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class BluePotion : IItem
{
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

    public BluePotion(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;   
    }

    public void itemSpawn(SpriteDict bluepotionDict, Point spawnPosition, CollisionController collisionController)
    {
        bluepotionCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        bluepotionDict.Position = spawnPosition;
        bluepotionDict.SetSprite("potion_blue");
        collisionController.AddCollidable(bluepotionCollidable);
    }
}