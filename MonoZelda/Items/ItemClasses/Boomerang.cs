using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision.Collidables;

namespace MonoZelda.Items.ItemClasses;

public class Boomerang : IItem
{
    private ICollidable boomerangCollidable;
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
        boomerangCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 28),graphicsDevice);
        collisionController.AddCollidable(boomerangCollidable);
        boomerangCollidable.setSpriteDict(boomerangDict);
        boomerangDict.Position = spawnPosition;
        boomerangDict.SetSprite("boomerang");
    }

}
