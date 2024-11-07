using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Tiles;
using System;
using System.Collections.Generic;

namespace MonoZelda.Trigger
{
    internal class PushBlockTrigger : TriggerCollidable
    {
        private static readonly int PUSH_DELAY = 30;
        private int pushCounter = 0;
        private Point destination;
        private CollisionController collisionManager;
        private readonly ICollidable staticCollider;
        private Direction pushDirection;
        private SpriteDict blockDict;

        // Overriding Bounds to enforce this trigger collider follows the static collider we create
        public new Rectangle Bounds
        {
            get { return staticCollider.Bounds; }
            set { staticCollider.Bounds = value; }
        }

        public PushBlockTrigger(CollisionController colliderManager, Point position)
            : base(new Rectangle(position, new Point(64, 64)))
        {
            collisionManager = colliderManager;

            var rect = new Rectangle(position, new Point(64, 64));

            staticCollider = new StaticRoomCollidable(rect);
            colliderManager.AddCollidable(staticCollider);

            // The trigger collider sits on top of the static collider
            colliderManager.AddCollidable(this);

            blockDict = new SpriteDict(SpriteType.Blocks, 0, new Point(0, 0));
            blockDict.SetSprite(BlockType.tile_block2.ToString());
            blockDict.Position = Bounds.Location;
            setSpriteDict(blockDict);

            destination = staticCollider.Bounds.Location;

            OnTrigger += PushTrigger;
        }

        // Need to make Intersect use our static collider Bounds proxy.
        public new bool Intersects(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public new Rectangle GetIntersectionArea(ICollidable other)
        {
            return Rectangle.Intersect(Bounds, other.Bounds);
        }

        public void Update()
        {
            blockDict.Position = Bounds.Location;

            if (Bounds.Location != destination)
            {
                Bounds = new Rectangle(
                    staticCollider.Bounds.Location - DirectionToVector(pushDirection).ToPoint(),
                    staticCollider.Bounds.Size
                );
            }

            // Naturally decay the push counter
            pushCounter = Math.Max(0, pushCounter - 1);
        }

        public void PushTrigger(Direction direction)
        {
            if(pushCounter > PUSH_DELAY)
            {
                // Send the block away from the player
                destination = Bounds.Location - DirectionToVector(direction, 64).ToPoint();

                // We don't want trigger this again
                UnregisterHitbox();
                collisionManager.RemoveCollidable(this);
                OnTrigger -= PushTrigger; // Allways clean up your events
            }

            // +2 to overcome the -1 on update
            pushCounter += 2;
            pushDirection = direction;
        }

        private Vector2 DirectionToVector(Direction direction, int magnatude = 1)
        {
            return direction switch
            {
                Direction.Up => new Vector2(0, -1 * magnatude),
                Direction.Down => new Vector2(0, 1 * magnatude),
                Direction.Left => new Vector2(-1 * magnatude, 0),
                Direction.Right => new Vector2(1 * magnatude, 0),
                _ => Vector2.Zero
            };
        }
    }
}
