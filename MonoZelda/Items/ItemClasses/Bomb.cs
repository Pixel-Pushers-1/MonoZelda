using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : IItem
{
    private CollisionController collisionController;
    private Collidable bombCollidable;
    private GraphicsDevice graphicsDevice;
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

    public Bomb(GraphicsDevice graphicsDevice)  
    {
        this.collisionController = collisionController;
        this.graphicsDevice = graphicsDevice;
    }

    public void itemSpawn(SpriteDict bombDict, Point spawnPosition, CollisionController collisionController)
    {
        bombCollidable = new Collidable(new Rectangle(spawnPosition.X,spawnPosition.Y, 32, 64), graphicsDevice, CollidableType.Item);
        bombDict.Position = spawnPosition;
        bombDict.SetSprite("bomb");
        collisionController.AddCollidable(bombCollidable);
    }

}