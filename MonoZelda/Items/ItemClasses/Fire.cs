using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;

namespace MonoZelda.Items.ItemClasses
{
    public class Fire : IItem
    {
        private ItemCollidable fireCollidable;
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

        public void itemSpawn(SpriteDict fireDict, Point spawnPosition, CollisionController collisionController)
        {
            //adjusted because can overlay things on top of each other
            Point adjustedPosition = new Point(spawnPosition.X, spawnPosition.Y + 64);
            fireDict.Position = adjustedPosition;
            fireDict.SetSprite("fire");
        }
    }
}