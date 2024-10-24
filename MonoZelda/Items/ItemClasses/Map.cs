using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Map : IItem
{
    private ItemCollidable mapCollidable;
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

    public Map(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict mapDict, Point spawnPosition, CollisionController collisionController)
    {
        mapCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice, ItemList.Map);
        collisionController.AddCollidable(mapCollidable);
        mapCollidable.setSpriteDict(mapDict);
        mapDict.Position = spawnPosition;
        mapDict.SetSprite("map");
    }
}

