using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;

namespace MonoZelda.Tiles
{
    internal class BombableWall : DungeonDoor, ICollidable
    {
        private const int HALF_TILE = 16;
        private ICollidable collider;
        private CollisionHitboxDraw hitbox;
        private bool isOpen;

        public BombableWall(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c)
            : base(spawnPoint, roomTransitionCommand, c)
        {
            hitbox = new CollisionHitboxDraw(this);
            
            isOpen = spawnPoint.Type is 
                Dungeon1Sprite.wall_bombed_east or 
                Dungeon1Sprite.wall_bombed_north or 
                Dungeon1Sprite.wall_bombed_south or 
                Dungeon1Sprite.wall_bombed_west;
            
            if (!isOpen)
                MakeBombable();
            else
                SetMaskSprite(GetBombedMaskSprite());
        }

        private void MakeBombable()
        {
            hitbox = new CollisionHitboxDraw(this);
            CollisionController.AddCollidable(this);
            
            // Create collider to block entry, HALF_TILE makes the door flush with the wall
            var offset = Spawn.Direction == DoorDirection.North ? 
                new Point(0, -Spawn.Bounds.Size.Y / 2 + HALF_TILE) : new Point(0, 0);
            
            var bounds = new Rectangle(Spawn.Position + offset, Spawn.Bounds.Size);
            
            collider = new StaticRoomCollidable(bounds);
            CollisionController.AddCollidable(collider);
        }

        protected override Dungeon1Sprite GetMaskSprite()
        {
            if (isOpen)
            {
                return GetBombedSprite();
            }
            
            return Direction switch
            {
                DoorDirection.North => Dungeon1Sprite.doorframe_wall_north,
                DoorDirection.South => Dungeon1Sprite.doorframe_wall_south,
                DoorDirection.West => Dungeon1Sprite.doorframe_wall_west,
                DoorDirection.East => Dungeon1Sprite.doorframe_wall_east,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Dungeon1Sprite GetBombedMaskSprite()
        {
            return Direction switch
            {
                DoorDirection.North => Dungeon1Sprite.doorframe_bombed_north,
                DoorDirection.South => Dungeon1Sprite.doorframe_bombed_south,
                DoorDirection.West => Dungeon1Sprite.doorframe_bombed_west,
                DoorDirection.East => Dungeon1Sprite.doorframe_bombed_east,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Dungeon1Sprite GetBombedSprite()
        {
            return Spawn.Type switch
            {
                Dungeon1Sprite.bombable_wall_east => Dungeon1Sprite.wall_bombed_east,
                Dungeon1Sprite.bombable_wall_north => Dungeon1Sprite.wall_bombed_north,
                Dungeon1Sprite.bombable_wall_south => Dungeon1Sprite.wall_bombed_south,
                Dungeon1Sprite.bombable_wall_west => Dungeon1Sprite.wall_bombed_west,
                _ => throw new System.Exception("Invalid bombable wall type")
            };
        }

        public CollidableType type
        {
            get => CollidableType.Door;
            set => throw new InvalidOperationException();
        }

        public Rectangle Bounds
        {
            get => collider?.Bounds ?? Spawn.Bounds;
            set => collider.Bounds = value;
        }
        
        public SpriteDict CollidableDict { get; set; }
        public void UnregisterHitbox()
        {
            hitbox.Unregister();
        }

        public bool Intersects(ICollidable other)
        {
            if (other is PlayerProjectileCollidable collidable 
                && collidable.projectileType == ProjectileType.Bomb 
                && other.Bounds.Intersects(collider.Bounds))
            {
                BombWall();
            }

            return false;
        }

        private void BombWall()
        {
            PlayerState.Keyring.Add((Spawn.Destination, Spawn.Direction.Reverse()));

            isOpen = true;
            var newSprite = GetBombedSprite();
            SpriteDict.SetSprite(newSprite.ToString());
            Spawn.Type = newSprite;
            SetMaskSprite(GetBombedMaskSprite());
            UnregisterHitbox();

            CollisionController.RemoveCollidable(collider);
            CollisionController.RemoveCollidable(this);
        }

        public Rectangle GetIntersectionArea(ICollidable other)
        {
            return Rectangle.Intersect(Bounds, other.Bounds);
        }
    }
}