using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Map : Item
{
    public Map()
    {
        itemType = ItemList.Map;
    }
    public override void ItemSpawn(SpriteDict mapDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(mapDict, spawnPosition, collisionController);        
        mapDict.SetSprite("map");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}

