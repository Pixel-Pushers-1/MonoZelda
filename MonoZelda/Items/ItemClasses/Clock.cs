using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Clock : IItem
{
    private ItemCollidable clockCollidable;
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

    public void itemSpawn(SpriteDict clockDict, Point spawnPosition, CollisionController collisionController)
    {
        clockCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), ItemList.Clock);
        collisionController.AddCollidable(clockCollidable);
        clockCollidable.setSpriteDict(clockDict);
        clockDict.Position = spawnPosition;
        clockDict.SetSprite("clock");
    }

}
