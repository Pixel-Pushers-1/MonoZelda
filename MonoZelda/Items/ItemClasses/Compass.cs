using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private ItemCollidable compassCollidable;
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

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition, CollisionController collisionController)
    {
        compassCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), ItemList.Compass);
        collisionController.AddCollidable(compassCollidable);
        compassCollidable.setSpriteDict(compassDict);
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
    }
}
