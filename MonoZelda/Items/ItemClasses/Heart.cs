using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Heart : IItem
{
    private ItemCollidable heartCollidable;
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

    public void itemSpawn(SpriteDict heartDict, Point spawnPosition, CollisionController collisionController)
    {
        heartCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 28), ItemList.Heart);
        collisionController.AddCollidable(heartCollidable);
        heartCollidable.setSpriteDict(heartDict);
        heartDict.Position = spawnPosition;
        heartDict.SetSprite("heart_full");
    }

}