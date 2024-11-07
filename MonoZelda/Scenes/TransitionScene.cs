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
        }

        private SpriteDict CreateRoomBorderSprite(Texture2D texture, Point position)
        {
            var spriteDict = new SpriteDict(texture, SpriteCSVData.Blocks, SpriteLayer.Transition, position);
            spriteDict.SetSprite(nameof(Dungeon1Sprite.room_exterior));
            return spriteDict;
        }

        private SpriteDict CreateRoomSprite(Texture2D texture, string spriteName, Point position)
        {
            var spriteDict = new SpriteDict(texture, SpriteCSVData.Blocks, SpriteLayer.Transition, position);
            spriteDict.SetSprite(spriteName);
            return spriteDict;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // Load dungeon texture
            var dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);

            // Set up room and border sprites
            currentBorderSpriteDict = CreateRoomBorderSprite(dungeonTexture, DungeonConstants.DungeonPosition);
            nextBorderSpriteDict = CreateRoomBorderSprite(dungeonTexture, DungeonConstants.DungeonPosition + DungeonConstants.adjacentRoomSpawnPoints[TransitionDirection]);
            currentRoomSpriteDict = CreateRoomSprite(dungeonTexture, currentRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition);
            nextRoomSpriteDict = CreateRoomSprite(dungeonTexture, nextRoom.RoomSprite.ToString(), DungeonConstants.BackgroundPosition + DungeonConstants.adjacentRoomSpawnPoints[TransitionDirection]);

            // create list of spriteDicts
            spritesToMove = new List<SpriteDict> { currentBorderSpriteDict, nextBorderSpriteDict, currentRoomSpriteDict, nextRoomSpriteDict };

            // create Door spriteDicts
            foreach (var currentDoorSpawn in currentRoom.GetDoors())
            {
                SpriteDict doorDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Transition, currentDoorSpawn.Position);
                doorDict.SetSprite(currentDoorSpawn.Type.ToString());
                spritesToMove.Add(doorDict);
            }

            foreach (var nextDoorSpawn in nextRoom.GetDoors())
            {
                SpriteDict doorDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Transition, nextDoorSpawn.Position + DungeonConstants.adjacentRoomSpawnPoints[TransitionDirection]);
                doorDict.SetSprite(nextDoorSpawn.Type.ToString());
                spritesToMove.Add(doorDict);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 nextRoomPos = nextBorderSpriteDict.Position.ToVector2();
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
