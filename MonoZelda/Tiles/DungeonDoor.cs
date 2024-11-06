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

            // TODO: Setup trigger bounds loaction based on direction
            trigger = new TriggerCollidable(spawnPont.Bounds);
            trigger.OnTrigger += Transition;
            c.AddCollidable(trigger);

            trigger.OnTrigger += Transition;
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

        private void Transition(Direction d)
        {
            trigger.OnTrigger -= Transition;
            transitionCommand.Execute(spawnPont.Destination);
        }
    }
}
