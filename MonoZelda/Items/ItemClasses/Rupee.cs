using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Rupee : IItem
{
    private ItemCollidable rupeeCollidable;
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

    public void itemSpawn(SpriteDict rupeeDict, Point spawnPosition, CollisionController collisionController)
    {
        rupeeCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), ItemList.Rupee);
        collisionController.AddCollidable(rupeeCollidable);
        rupeeCollidable.setSpriteDict(rupeeDict);
        rupeeDict.Position = spawnPosition;
        rupeeDict.SetSprite("rupee");
    }

}