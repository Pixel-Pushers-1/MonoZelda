using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;

public class Arrow : IItem
{
    private ItemCollidable arrowCollidable;
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

    public void itemSpawn(SpriteDict arrowDict, Point spawnPosition, CollisionController collisionController)
    {
        arrowCollidable = new ItemCollidable(new Rectangle(spawnPosition.X, spawnPosition.Y, 32, 64), ItemList.Arrow);
        collisionController.AddCollidable(arrowCollidable);
        arrowCollidable.setSpriteDict(arrowDict);
        arrowDict.Position = spawnPosition;
        arrowDict.SetSprite("candle_blue");
    }
}