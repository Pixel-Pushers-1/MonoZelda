using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;
public class Bow : IItem
{
    private ItemCollidable bowCollidable;
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

    public void itemSpawn(SpriteDict bowDict, Point spawnPosition, CollisionController collisionController)
    {
        bowCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), ItemList.Bow);
        collisionController.AddCollidable(bowCollidable);
        bowCollidable.setSpriteDict(bowDict);
        bowDict.Position = spawnPosition;
        bowDict.SetSprite("bow");
    }

}