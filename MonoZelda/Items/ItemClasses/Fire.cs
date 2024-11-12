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


        public void itemSpawn(SpriteDict fireDict, Point spawnPosition, CollisionController collisionController)
        {
            //adjusted because can overlay things on top of each other
            Point adjustedPosition = new Point(spawnPosition.X, spawnPosition.Y + 64);
            fireDict.Position = adjustedPosition;
            fireDict.SetSprite("fire");
        }
    }
}