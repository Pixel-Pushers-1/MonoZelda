using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Fairy : IItem
{
    private ItemCollidable fairyCollidable;
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

    public Fairy(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict fairyDict, Point spawnPosition, CollisionController collisionController)
    {
        fairyCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice, ItemList.Fairy);
        collisionController.AddCollidable(fairyCollidable);
        fairyCollidable.setSpriteDict(fairyDict);
        fairyDict.Position = spawnPosition;
        fairyDict.SetSprite("fairy");
    }

}
