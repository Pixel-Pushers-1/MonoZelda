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
        private SpriteDict currentBorderSpriteDict;
        private SpriteDict nextBorderSpriteDict;
        private SpriteDict currentRoomSpriteDict;
        private SpriteDict nextRoomSpriteDict;
        private List<SpriteDict> spritesToMove;

        private readonly Dictionary<Direction, Point> directionShiftMap = new()
        {
            { Direction.Left, new Point(8,0) },
            { Direction.Right, new Point(-8,0) },
            { Direction.Up, new Point(0,8) },
            { Direction.Down, new Point(0,-8) },
        };

        public TransitionScene(IDungeonRoom currentRoom, IDungeonRoom nextRoom, ICommand loadCommand, Direction doorCollisionDirection)
        {
            this.loadCommand = loadCommand;
            this.currentRoom = currentRoom;
            this.nextRoom = nextRoom;
            TransitionDirection = doorCollisionDirection;
            spritesToMove = new List<SpriteDict>();
        }

        private void CreateSpriteDict(Texture2D texture, string spriteName, Point position)
        {
            var spriteDict = new SpriteDict(texture, SpriteCSVData.Blocks, SpriteLayer.Transition, position);
            spriteDict.SetSprite(spriteName);
            spritesToMove.Add(spriteDict);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // Load dungeon texture
            var dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);

            // Set up room and border sprites
            CreateSpriteDict(dungeonTexture, "room_exterior", DungeonConstants.DungeonPosition);
            CreateSpriteDict(dungeonTexture, "room_exterior", DungeonConstants.DungeonPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);
            CreateSpriteDict(dungeonTexture, currentRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition);
            CreateSpriteDict(dungeonTexture, nextRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);

            // create Door spriteDicts
            foreach (var currentDoorSpawn in currentRoom.GetDoors())
            {
                CreateSpriteDict(dungeonTexture, currentDoorSpawn.Type.ToString(), currentDoorSpawn.Position);
            }

            foreach (var nextDoorSpawn in nextRoom.GetDoors())
            {
                CreateSpriteDict(dungeonTexture, nextDoorSpawn.Type.ToString(), nextDoorSpawn.Position + DungeonConstants.adjacentTransitionRoomSpawnPoints[TransitionDirection]);
            }
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
