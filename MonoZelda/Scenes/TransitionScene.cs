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
            spritesToMove = new List<SpriteDict> ();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // load dungeon texture
            var dungeonTexture = contentManager.Load<Texture2D>(TextureData.Blocks);

            // load border sprite
            currentBorderSpriteDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Transition, DungeonConstants.DungeonPosition);
            currentBorderSpriteDict.SetSprite(nameof(Dungeon1Sprite.room_exterior));
            nextBorderSpriteDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Transition, DungeonConstants.DungeonPosition + DungeonConstants.adjacentRoomSpawnPoints[TransitionDirection]);
            nextBorderSpriteDict.SetSprite(nameof(Dungeon1Sprite.room_exterior));

            // create and set spriteDicts for transition rooms
            currentRoomSpriteDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks, SpriteLayer.Transition, DungeonConstants.BackgroundPosition);
            currentRoomSpriteDict.SetSprite(currentRoom.RoomSprite.ToString());
            nextRoomSpriteDict = new SpriteDict(dungeonTexture, SpriteCSVData.Blocks,SpriteLayer.Transition, DungeonConstants.BackgroundPosition + DungeonConstants.adjacentRoomSpawnPoints[TransitionDirection]);
            nextRoomSpriteDict.SetSprite(nextRoom.RoomSprite.ToString());

            // add all spriteDicts to update list
            spritesToMove.Add(currentBorderSpriteDict);
            spritesToMove.Add(nextBorderSpriteDict);
            spritesToMove.Add(currentRoomSpriteDict);
            spritesToMove.Add(nextRoomSpriteDict);
        }

        public override void Update(GameTime gameTime)
        {
            // update spriteDict positions
            Vector2 nextRoomPos = nextBorderSpriteDict.Position.ToVector2();
            Vector2 sceneRoomPos = DungeonConstants.DungeonPosition.ToVector2();
            if(Vector2.Distance(nextRoomPos,sceneRoomPos) > 0)
            {
                System.Diagnostics.Debug.WriteLine(Vector2.Distance(nextRoomPos, sceneRoomPos));
                foreach(var spriteDict in spritesToMove)
                {
                    spriteDict.Position -= directionShiftMap[TransitionDirection];
                }
            }
            else
            {
                // TODO: If done, 
                loadCommand.Execute(nextRoom.RoomName);
            }
        }
    }
}
