using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Scenes
{
    internal class MarioLevelTransitionScene : Scene
    {
        private readonly GraphicsDevice gd;
        private readonly DungeonScene scene;
        private readonly IDungeonRoom room;
        private readonly ICommand loadCommand;

        private SpriteDict leftCurtain;
        private SpriteDict rightCurtain;

        private int delay;

        public MarioLevelTransitionScene(DungeonScene scene, IDungeonRoom room, ICommand loadCommand, GraphicsDevice gd)
        {
            this.scene = scene;
            this.gd = gd;
            this.room = room;
            this.loadCommand = loadCommand;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (room.RoomName == DungeonScene.MARIO_ROOM)
            {
                EnterMarioScene();
            }
            else
            {
                ExitMarioScene();
            }
            
            // center of the screen
            var center = new Point(gd.Viewport.Width / 2, DungeonConstants.RoomPosition.Y);

            // the room texutres are 192 * 4 = 768 pixels wide
            var leftPosition = center - new Point(192 * 4, 0);
            leftCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, leftPosition);
            leftCurtain.SetSprite(nameof(Dungeon1Sprite.room_41));

            rightCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, center);
            rightCurtain.SetSprite(nameof(Dungeon1Sprite.room_41));
        }

        private void EnterMarioScene()
        {
            var offset = new Point(0, 64);
            var background = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition+ offset);
            background.SetSprite(nameof(Dungeon1Sprite.room_item));

            PlayerState.Position = DungeonConstants.DungeonPosition + new Point(224, 96);
        }
        
        private void ExitMarioScene()
        {
            var roomBg = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.BackgroundPosition);
            roomBg.SetSprite(room.RoomSprite.ToString());
            
            // Room wall border
            var r = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition);
            r.SetSprite(nameof(Dungeon1Sprite.room_exterior));
            r.Position = DungeonConstants.DungeonPosition;
            
            foreach (var door in room.GetDoors())
            {
                var spriteDict = new SpriteDict(SpriteType.Blocks, SpriteLayer.DoorLayer, door.Position);
                spriteDict.SetSprite(door.Type.ToString());
            }
            
            
            PlayerState.Position = DungeonConstants.DungeonPosition + new Point(416, 480);
        }
        
        public override void Update(GameTime gameTime)
        {
            // It takes a few frames before the textures load
            if(delay < 15)
            {
                delay++;
                return;
            }

            var moveSpeed = 10;
            leftCurtain.Position = new Point(leftCurtain.Position.X - moveSpeed, leftCurtain.Position.Y);
            rightCurtain.Position = new Point(rightCurtain.Position.X + moveSpeed, rightCurtain.Position.Y);

            if (rightCurtain.Position.X > gd.Viewport.Width)
            {
                loadCommand.Execute(room.RoomName);
            }
        }
    }
}
