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
        private const int SAFE_EDGE = 16;
        
        public DoorDirection Direction => spawnPont.Direction;
        public Point Position { get; set; }
        private ICommand transitionCommand;

        private SpriteDict spriteDict;
        private TriggerCollidable trigger;
        private DoorSpawn spawnPont;

        // So many arguments, but it's necessary for the door to be able to transition to the next room, open to suggetions -js
        public DungeonDoor(DoorSpawn spawnPoint, ICommand roomTransitionCommand, CollisionController c)
        {
            spawnPont = spawnPoint;

            transitionCommand = roomTransitionCommand;

            spriteDict = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, spawnPont.Position);
            spriteDict.SetSprite(spawnPont.Type.ToString());

            trigger = CreateActivateTrigger();
            c.AddCollidable(trigger);
            
            AddDoorMask();
        }

        private void AddDoorMask()
        {
            var mask = new SpriteDict(SpriteType.Blocks, SpriteLayer.Player + 1, spawnPont.Position);
            switch (Direction)
            {
                case DoorDirection.North:
                    mask.SetSprite(nameof(Dungeon1Sprite.doorframe_door_north));
                    break;
                case DoorDirection.South:
                    mask.SetSprite(nameof(Dungeon1Sprite.doorframe_door_south));
                    break;
                case DoorDirection.West:
                    mask.SetSprite(nameof(Dungeon1Sprite.doorframe_door_west));
                    break;
                case DoorDirection.East:
                    mask.SetSprite(nameof(Dungeon1Sprite.doorframe_door_east));
                    break;
            }
        }

        private TriggerCollidable CreateActivateTrigger()
        {
            // Move the trigger off the screen by one door size - SAFE_EDGE to prevent the player
            // from slipping around the door
            var offset = spawnPont.Direction switch
            {
                DoorDirection.North => new Point(0, -spawnPont.Bounds.Size.Y + SAFE_EDGE),
                DoorDirection.South => new Point(0, spawnPont.Bounds.Size.Y - SAFE_EDGE),
                DoorDirection.West => new Point(-spawnPont.Bounds.Size.X + SAFE_EDGE, 0),
                DoorDirection.East => new Point(spawnPont.Bounds.Size.X - SAFE_EDGE, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var bounds = new Rectangle(spawnPont.Position + offset, spawnPont.Bounds.Size);
            
            trigger = new TriggerCollidable(bounds);
            trigger.OnTrigger += Transition;
            
            return trigger;
        }

        public void Open()
        {
            switch(Direction)
            {
                case DoorDirection.North:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_north));
                    break;
                case DoorDirection.South:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_south));
                    break;
                case DoorDirection.West:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_west));
                    break;
                case DoorDirection.East:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_open_east));
                    break;
            }
        }

        public void Close()
        {
            switch (Direction)
            {
                case DoorDirection.North:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_north));
                    break;
                case DoorDirection.South:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_south));
                    break;
                case DoorDirection.West:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_west));
                    break;
                case DoorDirection.East:
                    spriteDict.SetSprite(nameof(Dungeon1Sprite.door_closed_east));
                    break;
            }
        }

        private void Transition(Direction transitionDirection)
        {
            trigger.OnTrigger -= Transition;
            transitionCommand.Execute(spawnPont.Destination,transitionDirection);
        }
    }
}
