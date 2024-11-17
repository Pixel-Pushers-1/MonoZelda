using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Link;
using MonoZelda.Sprites;
using MonoZelda.Tiles;

namespace MonoZelda.Dungeons
{
    internal class DungeonDoor : IDoor
    {
        internal const int SAFE_EDGE = 16;
        internal SpriteDict SpriteDict;
        internal TriggerCollidable DoorTrigger;
        internal DoorSpawn Spawn;
        internal CollisionController CollisionController;
        
        
        public DoorDirection Direction => Spawn.Direction;
        public Point Position { get; set; }
        
        private ICommand transitionCommand;
        private SpriteDict doorMask;
        
        // So many arguments, but it's necessary for the door to be able to transition to the next room, open to suggetions -js
        public DungeonDoor(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c)
        {
            Spawn = spawnPoint;
            transitionCommand = roomTransitionCommand;
            CollisionController = c;
            
            SpriteDict = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, Spawn.Position);
            SpriteDict.SetSprite(Spawn.Type.ToString());

            DoorTrigger = CreateActivateTrigger();
            c.AddCollidable(DoorTrigger);
            
            AddDoorMask();
        }

        private void AddDoorMask()
        {
            doorMask = new SpriteDict(SpriteType.Blocks, SpriteLayer.Player + 1, Spawn.Position);
            var sprite = GetMaskSprite();
            doorMask.SetSprite(sprite.ToString());
        }
        
        public void SetMaskSprite(Dungeon1Sprite sprite)
        {
            doorMask.SetSprite(sprite.ToString());
        }
        
        protected virtual Dungeon1Sprite GetMaskSprite()
        {
            return Direction switch
            {
                DoorDirection.North => Dungeon1Sprite.doorframe_door_north,
                DoorDirection.South => Dungeon1Sprite.doorframe_door_south,
                DoorDirection.West => Dungeon1Sprite.doorframe_door_west,
                DoorDirection.East => Dungeon1Sprite.doorframe_door_east,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private TriggerCollidable CreateActivateTrigger()
        {
            var bounds = GetDoorBounds();
            
            DoorTrigger = new TriggerCollidable(bounds);
            DoorTrigger.OnTrigger += Transition;
            
            return DoorTrigger;
        }

        private Rectangle GetDoorBounds()
        {
            // Move the trigger off the screen by one door size - SAFE_EDGE to prevent the player
            // from slipping around the door
            var offset = Spawn.Direction switch
            {
                DoorDirection.North => new Point(0, -Spawn.Bounds.Size.Y + SAFE_EDGE),
                DoorDirection.South => new Point(0, Spawn.Bounds.Size.Y - SAFE_EDGE),
                DoorDirection.West => new Point(-Spawn.Bounds.Size.X + SAFE_EDGE, 0),
                DoorDirection.East => new Point(Spawn.Bounds.Size.X - SAFE_EDGE, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            return new Rectangle(Spawn.Position + offset, Spawn.Bounds.Size);
        }

        internal void ResetDoorTrigger()
        {
            if (DoorTrigger == null) return;
            
            DoorTrigger.Bounds = GetDoorBounds();
        }

        public void Open()
        {
            switch(Direction)
            {
                case DoorDirection.North:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_north));
                    break;
                case DoorDirection.South:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_south));
                    break;
                case DoorDirection.West:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_west));
                    break;
                case DoorDirection.East:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_east));
                    break;
            }
        }

        public void Close()
        {
            switch (Direction)
            {
                case DoorDirection.North:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_north));
                    break;
                case DoorDirection.South:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_south));
                    break;
                case DoorDirection.West:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_west));
                    break;
                case DoorDirection.East:
                    SpriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_east));
                    break;
            }
        }

        protected virtual void Transition(Direction transitionDirection)
        {
            DoorTrigger.OnTrigger -= Transition;
            transitionCommand.Execute(Spawn.Destination, transitionDirection);
        }
    }
}
