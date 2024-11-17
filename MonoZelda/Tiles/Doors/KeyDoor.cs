using System;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using MonoZelda.Sound;

namespace MonoZelda.Tiles
{
    internal class KeyDoor : DungeonDoor
    {
        private const int HALF_TILE = 32;
        private DoorSpawn spawn;
        private ICollidable collider;
        private TriggerCollidable trigger;
        private bool isOpen;
        
        public KeyDoor(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c) 
            : base(spawnPoint, roomTransitionCommand, c)
        {
            BlockDoor(spawnPoint, c);
        }

        private void BlockDoor(DoorSpawn spawnPoint, CollisionController c)
        {
            if(collider != null)
            {
                c.RemoveCollidable(collider);
            }

            // Create collider to block entry, HALF_TILE makes the door flush with the wall
            var offset = spawnPoint.Direction == DoorDirection.North ?
                new Point(0, -spawnPoint.Bounds.Size.Y / 2 + HALF_TILE) : new Point(0, 0);

            var bounds = new Rectangle(spawnPoint.Position + offset, spawnPoint.Bounds.Size);
            collider = new StaticRoomCollidable(bounds);
            c.AddCollidable(collider);

            // Move the underlying trigger to match
            DoorTrigger.Bounds = bounds;
        }

        protected override void Transition(Direction transitionDirection)
        {
            if(!isOpen && PlayerState.Keys > 0)
            {
                Unlock(Spawn.Direction);
            }
            else
            {
                base.Transition(transitionDirection);
            }
        }

        private void Unlock(DoorDirection transitionDirection)
        {
            PlayerState.Keys--;
            PlayerState.Keyring.Add((Spawn.Destination, transitionDirection.Reverse()));
            var openSprite = GetOpenSprite();
            SpriteDict.SetSprite(openSprite.ToString());
            Spawn.Type = openSprite;
            ResetDoorTrigger();
            SoundManager.PlaySound("LOZ_Door_Unlock", false);
            CollisionController.RemoveCollidable(collider);
            isOpen = true;
        }

        private Dungeon1Sprite GetOpenSprite()
        {
            return Spawn.Type switch
            {
                Dungeon1Sprite.door_locked_east => Dungeon1Sprite.door_open_east,
                Dungeon1Sprite.door_locked_north => Dungeon1Sprite.door_open_north,
                Dungeon1Sprite.door_locked_south => Dungeon1Sprite.door_open_south,
                Dungeon1Sprite.door_locked_west => Dungeon1Sprite.door_open_west,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
