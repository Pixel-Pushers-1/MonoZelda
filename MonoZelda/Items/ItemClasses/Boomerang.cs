using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Items.ItemClasses;

public class Boomerang : IItem
{
    private ItemCollidable boomerangCollidable;
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

    public Boomerang(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict boomerangDict, Point spawnPosition, CollisionController collisionController)
    {
        boomerangCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 28), graphicsDevice, ItemList.Boomerang);
        collisionController.AddCollidable(boomerangCollidable);
        boomerangCollidable.setSpriteDict(boomerangDict);
        boomerangDict.Position = spawnPosition;
        boomerangDict.SetSprite("boomerang");
    }

}
