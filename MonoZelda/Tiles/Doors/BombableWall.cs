using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Tiles
{
    internal class BombableWall : DungeonDoor, IDisposable
    {
        private const int HALF_TILE = 16;
        private ProjectileManager pm;
        private ICollidable collider;

        public BombableWall(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c,
            ProjectileManager pm)
            : base(spawnPoint, roomTransitionCommand, c)
        {
            // Create collider to block entry, HALF_TILE makes the door flush with the wall
            var offset = spawnPoint.Direction == DoorDirection.North ? 
                new Point(0, -spawnPoint.Bounds.Size.Y / 2 + HALF_TILE) : new Point(0, 0);
            
            var bounds = new Rectangle(spawnPoint.Position + offset, spawnPoint.Bounds.Size);
            
            collider = new StaticBoundaryCollidable(bounds);
            c.AddCollidable(collider);
            
            this.pm = pm;
            pm.OnProjectileColliderActive += OnProjectileColliderActive;
        }

        private void OnProjectileColliderActive(PlayerProjectileCollidable collidable)
        {
            if (collidable.projectileType == ProjectileType.Bomb && Spawn.Bounds.Intersects(collidable.Bounds))
            {
                SpriteDict.SetSprite(GetBombedSprite().ToString());
                CollisionController.RemoveCollidable(collider);
                
                pm.OnProjectileColliderActive -= OnProjectileColliderActive;
            }
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

        public void Dispose()
        {
            pm.OnProjectileColliderActive -= OnProjectileColliderActive;
        }
    }
}