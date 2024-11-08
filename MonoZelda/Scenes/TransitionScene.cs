using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.Scenes
{
    internal class TransitionScene : Scene
    {
        private ICommand loadCommand;

        private IDungeonRoom currentRoom;
        private IDungeonRoom nextRoom;
        private Direction TransitionDirection;
        private List<SpriteDict> spritesToMove;

        private readonly Dictionary<Direction, Point> directionShiftMap = new()
        {
            { Direction.Left, new Point(8,0) },
            { Direction.Right, new Point(-8,0) },
            { Direction.Up, new Point(0,8) },
            { Direction.Down, new Point(0,-8) },
        };

        public TransitionScene(IDungeonRoom currentRoom, IDungeonRoom nextRoom, ICommand loadCommand, Direction transitionDirection)
        {
            this.loadCommand = loadCommand;
            this.currentRoom = currentRoom;
            this.nextRoom = nextRoom;
            TransitionDirection = transitionDirection;
            spritesToMove = new List<SpriteDict>();
        }

        private void CreateSpriteDict(string spriteName, Point position)
        {
            var spriteDict = new SpriteDict(SpriteType.Blocks,SpriteLayer.Transition, position);
            spriteDict.SetSprite(spriteName);
            spritesToMove.Add(spriteDict);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // Set up room and border sprites
            CreateSpriteDict("room_exterior", DungeonConstants.DungeonPosition);
            CreateSpriteDict("room_exterior", DungeonConstants.DungeonPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);
            CreateSpriteDict(currentRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition);
            CreateSpriteDict(nextRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);

            // create Door spriteDicts
            foreach (var currentDoorSpawn in currentRoom.GetDoors())
            {
                CreateSpriteDict(currentDoorSpawn.Type.ToString(), currentDoorSpawn.Position);
            }

            foreach (var nextDoorSpawn in nextRoom.GetDoors())
            {
                CreateSpriteDict(nextDoorSpawn.Type.ToString(), nextDoorSpawn.Position + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);
            }

            // create Fake Link

        }

        public override void Update(GameTime gameTime)
        {
            Vector2 nextRoomPos = spritesToMove[1].Position.ToVector2();
            Vector2 sceneRoomPos = DungeonConstants.DungeonPosition.ToVector2();
            float distance = Vector2.Distance(nextRoomPos, sceneRoomPos);

            if (distance > 0)
            {
                Point shift = directionShiftMap[TransitionDirection];
                foreach (var spriteDict in spritesToMove)
                {
                    spriteDict.Position -= shift;
                }
            }
            else
            {
            loadCommand.Execute(nextRoom.RoomName);
        }
    }
}
}
