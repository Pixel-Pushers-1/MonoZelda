using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;
public class Bomb : Item
{
    public Bomb()
    {
        itemType = ItemList.Bomb;
    }

    public override void ItemSpawn(SpriteDict bombDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(bombDict, spawnPosition, collisionController);  
        bombDict.SetSprite("bomb");   
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}