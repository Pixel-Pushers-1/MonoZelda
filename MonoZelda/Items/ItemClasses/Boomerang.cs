using Microsoft.Xna.Framework.Graphics;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

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

    public Boomerang(CollisionController collisionController, GraphicsDevice graphicsDevice)
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict boomerangDict, Point spawnPosition)
    {
        boomerangCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 32), graphicsDevice, "Boomerang");
        boomerangDict.Position = spawnPosition;
        boomerangDict.SetSprite("boomerang");
        collisionController.AddCollidable(boomerangCollidable);
    }

}
