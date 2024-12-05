using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Sprites;

namespace MonoZelda.Doors
{
    internal class Wall : IDoor
    {
        private const int HALF_TILE = 32;
        public Wall(DoorSpawn door, CollisionController c)
        {
            var sprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background + 1, door.Position);
            sprite.SetSprite(door.Type.ToString());

            // Create collider to block entry, HALF_TILE makes the door flush with the wall
            var offset = door.Direction == DoorDirection.North ?
                new Point(0, -door.Bounds.Size.Y / 2 + HALF_TILE) : new Point(0, 0);

            var bounds = new Rectangle(door.Position + offset, door.Bounds.Size);

            var collider = new StaticRoomCollidable(bounds);
            c.AddCollidable(collider);
        }

        public void Open()
        {
        }

        public void Close()
        {
        }
    }
}
