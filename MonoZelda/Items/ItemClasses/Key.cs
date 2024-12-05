using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;
using MonoZelda.Collision;

namespace MonoZelda.Items.ItemClasses;

public class Key : Item
{
    public Key(ItemManager itemManager) : base(itemManager)
    {
        itemType = ItemList.Key;
    }

    public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
    {
        // create item SpriteDict
        itemDict = new SpriteDict(SpriteType.Items, SpriteLayer.HUD - 3, itemSpawn.Position);
        itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);
        itemDict.SetSprite("key_0");

        // create item Collidable 
        itemBounds = new Rectangle(itemSpawn.Position, new Point(24, 56));
        itemCollidable = new ItemCollidable(itemBounds, itemType);
        itemCollidable.setItem(this);
        collisionController.AddCollidable(itemCollidable);

        // store itemSpawn for removal
        this.itemSpawn = itemSpawn;
    }

    public override void HandleCollision(CollisionController collisionController)
    {
        PlayerState.AddKeys(1);
        SoundManager.PlaySound("LOZ_Get_Item", false);
        base.HandleCollision(collisionController);
    }
}
