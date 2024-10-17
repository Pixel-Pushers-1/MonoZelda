using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Heart : IItem
{
    private Collidable heartCollidable;
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

    public Heart(GraphicsDevice graphicsDevice)
    { 
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict heartDict, Point spawnPosition, CollisionController collisionController)
    {
        heartCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(heartCollidable);
        heartCollidable.setSpriteDict(heartDict);
        heartDict.Position = spawnPosition;
        heartDict.SetSprite("heart_full");
    }

}