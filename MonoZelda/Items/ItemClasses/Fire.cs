using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Enemies;
using MonoZelda.Link;
using System.Collections.Generic;

namespace MonoZelda.Items.ItemClasses
{
    public class Fire : Item
    {
        public Fire(List<Enemy> roomEnemyList, PlayerCollisionManager playerCollision, List<Item> updateList) : base(roomEnemyList, playerCollision, updateList)
        {
            itemType = ItemList.Fire;
        }

        public override void ItemSpawn(SpriteDict fireDict, Point spawnPosition, CollisionController collisionController)
        {
            base.ItemSpawn(fireDict, spawnPosition, collisionController);
            fireDict.SetSprite("fire");
        }
    }
}