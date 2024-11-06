﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Dungeons;
using MonoZelda.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Scenes
{
    internal class EnterDungeonScene : Scene
    {
        private IDungeonRoom room;
        private GraphicsDevice gd;
        private DungeonScene scene;

        private SpriteDict leftCurtain;
        private SpriteDict rightCurtain;

        private int delay;

        public EnterDungeonScene(DungeonScene scene, IDungeonRoom room, GraphicsDevice gd)
        {
            this.scene = scene;
            this.room = room;
            this.gd = gd;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            // center of the creen
            var center = new Point(gd.Viewport.Width / 2, DungeonConstants.RoomPosition.Y);

            // Fake the dungeon entrance
            // Room wall border
            var r = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition);
            r.SetSprite(nameof(Dungeon1Sprite.room_exterior));
            r.Position = DungeonConstants.DungeonPosition;

            // Floor background
            var f = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.BackgroundPosition);
            f.SetSprite(room.RoomSprite.ToString());

            // the room texutres are 192 * 4 = 768 pixels wide
            var leftPosition = center - new Point(192 * 4, 0);
            leftCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, leftPosition);
            leftCurtain.SetSprite(nameof(Dungeon1Sprite.room_41));

            rightCurtain = new SpriteDict(SpriteType.Blocks, SpriteLayer.HUD, center);
            rightCurtain.SetSprite(nameof(Dungeon1Sprite.room_41));

            CreateFakeDoors(room);
        }

        private void CreateFakeDoors(IDungeonRoom room)
        {
            foreach(var door in room.GetDoors())
            {
                var doorSprite = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, door.Position);
                doorSprite.SetSprite(door.Type.ToString());
            }
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
                scene.LoadRoom(room.RoomName);
            }
        }
    }
}