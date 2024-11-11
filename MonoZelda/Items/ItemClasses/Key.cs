using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Items.ItemClasses;

public class Key : Item
{
    public Key()
    {
        itemType = ItemList.Key;
    }

    public override void ItemSpawn(SpriteDict keyDict, Point spawnPosition, CollisionController collisionController)
    {
        base.ItemSpawn(keyDict, spawnPosition, collisionController);    
        keyDict.SetSprite("key_0");
    }

    public override void HandleCollision(SpriteDict itemCollidableDict, CollisionController collisionController)
    {
        SoundManager.PlaySound("LOZ_Get_Key", false);
        base.HandleCollision(itemCollidableDict, collisionController);
    }
}
