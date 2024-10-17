using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : IItem
{
    private Collidable fairyCollidable;
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

    public Fairy(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict fairyDict, Point spawnPosition, CollisionController collisionController)
    {
        fairyCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        collisionController.AddCollidable(fairyCollidable);
        fairyCollidable.setSpriteDict(fairyDict);
        fairyDict.Position = spawnPosition;
        fairyDict.SetSprite("fairy");
    }

}
