using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Compass : IItem
{
    private Collidable compassCollidable;
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

    public Compass()
    {
    }

    public void itemSpawn(SpriteDict compassDict, Point spawnPosition, CollisionController collisionController)
    {
        compassCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 60, 60), CollidableType.Item);
        collisionController.AddCollidable(compassCollidable);
        compassCollidable.setSpriteDict(compassDict);
        compassDict.Position = spawnPosition;
        compassDict.SetSprite("compass");
    }
}
