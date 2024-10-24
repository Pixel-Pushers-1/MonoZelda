using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : IItem
{
    private ItemCollidable bombCollidable;
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

    public Bomb(GraphicsDevice graphicsDevice)  
    {
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict bombDict, Point spawnPosition, CollisionController collisionController)
    {
        bombCollidable = new ItemCollidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 28, 60), graphicsDevice, ItemList.Bomb);
        collisionController.AddCollidable(bombCollidable);
        bombCollidable.setSpriteDict(bombDict);
        bombDict.Position = spawnPosition;
        bombDict.SetSprite("bomb");   
    }

}