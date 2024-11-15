using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;
using MonoZelda.Dungeons;

namespace MonoZelda.Items.ItemClasses
{
    public class Fire : Item
    {
        private const float FLASHING_TIME = 0.75f;

        public Fire(ItemManager itemManager) : base(itemManager)
        {
            itemType = ItemList.Fire;
        }

        public override void ItemSpawn(ItemSpawn itemSpawn, CollisionController collisionController)
        {
            // fire offset for spawn
            Point offset = new Point(0, 64);

            // create item SpriteDict
            itemDict = new SpriteDict(SpriteType.Items, SpriteLayer.Items, itemSpawn.Position + offset);
            itemDict.SetFlashing(SpriteDict.FlashingType.OnOff, FLASHING_TIME);

            // create item Collidable
            itemBounds = new Rectangle(itemSpawn.Position, new Point(56, 56));
            itemCollidable = new ItemCollidable(itemBounds, itemType);
            collisionController.AddCollidable(itemCollidable);
        }

        public override void HandleCollision(CollisionController collisionController)
        {
            // do nothing
        }

    }
}