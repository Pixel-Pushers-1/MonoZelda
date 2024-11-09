
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Scenes;
using MonoZelda.Sound;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Enemies;

namespace MonoZelda.Tiles
{
    internal class DiamondDoor : DungeonDoor, IGameUpdate
    {
        private const int HALF_TILE = 32;
        
        private IDungeonRoom room;
        private bool isOpen;
        private ICollidable collider;
        private List<IEnemy> enemies;
        
        public DiamondDoor(DoorSpawn door, ICommand roomTransitionCommand, CollisionController c, List<IEnemy> enemies) 
            : base(door, roomTransitionCommand, c)
        {
            this.enemies = enemies;
            
            // Create collider to block entry, HALF_TILE makes the door flush with the wall
            var offset = door.Direction == DoorDirection.North ? 
                new Point(0, -door.Bounds.Size.Y / 2 + HALF_TILE) : new Point(0, 0);
            
            var bounds = new Rectangle(door.Position + offset, door.Bounds.Size);
            
            collider = new StaticBoundaryCollidable(bounds);
            c.AddCollidable(collider);
            
            // These doors close after the player enters
            SoundManager.PlaySound("LOZ_Door_Unlock", false);
            SpriteDict.SetSprite(GetClosedSprite().ToString());
        }

        private Dungeon1Sprite GetClosedSprite()
        {
            return Spawn.Type switch
            {
                Dungeon1Sprite.diamond_door_east => Dungeon1Sprite.door_closed_east,
                Dungeon1Sprite.diamond_door_north => Dungeon1Sprite.door_closed_north,
                Dungeon1Sprite.diamond_door_south => Dungeon1Sprite.door_closed_south,
                Dungeon1Sprite.diamond_door_west => Dungeon1Sprite.door_closed_west,
                _ => throw new InvalidEnumArgumentException()
            };
        }
        
        private Dungeon1Sprite GetOpenSprite()
        {
            return Spawn.Type switch
            {
                Dungeon1Sprite.diamond_door_east => Dungeon1Sprite.door_open_east,
                Dungeon1Sprite.diamond_door_north => Dungeon1Sprite.door_open_north,
                Dungeon1Sprite.diamond_door_south => Dungeon1Sprite.door_open_south,
                Dungeon1Sprite.diamond_door_west => Dungeon1Sprite.door_open_west,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        protected override void Transition(Direction transitionDirection)
        {
            if (isOpen)
            {
                base.Transition(transitionDirection);
            }
        }

        public void Update(GameTime time)
        {
            if (isOpen || CheckForEnemies()) return;
            
            SoundManager.PlaySound("LOZ_Door_Unlock", false);
            isOpen = true;
            SpriteDict.SetSprite(Spawn.Type.ToString());
            Spawn.Type = GetOpenSprite();
            CollisionController.RemoveCollidable(collider);
        }
        
        private bool CheckForEnemies()
        {
            return enemies.Count > 0;
        }
    }
}
