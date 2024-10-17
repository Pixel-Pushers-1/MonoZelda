using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Boomerang : IItem
{
    private CollisionController collisionController;
    private Collidable boomerangCollidable;
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

    public Boomerang(GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict boomerangDict, Point spawnPosition, CollisionController collisionController)
    {
        boomerangCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(boomerangCollidable);
        boomerangCollidable.setSpriteDict(boomerangDict);
        boomerangDict.Position = spawnPosition;
        boomerangDict.SetSprite("boomerang");
    }

}
