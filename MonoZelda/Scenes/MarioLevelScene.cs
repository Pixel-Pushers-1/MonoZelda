using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Collision;
using MonoZelda.Commands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;

namespace MonoZelda.Scenes
{
    internal class MarioLevelScene : RoomScene
    {
        private readonly CommandManager commandManager;
        private readonly CollisionController collisionController;
        private TriggerCollidable exit;
        
        public MarioLevelScene(GraphicsDevice graphicsDevice, CommandManager commandManager, CollisionController collisionController, IDungeonRoom room) 
            : base(graphicsDevice, commandManager, collisionController, room)
        {
            this.commandManager = commandManager;
            this.collisionController = collisionController;
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            // Load Mario level content
            var offset = new Point(0, 64);
            var background = new SpriteDict(SpriteType.Blocks, SpriteLayer.Background, DungeonConstants.DungeonPosition+ offset);
            background.SetSprite(nameof(Dungeon1Sprite.room_item));

            CreateStaticColliders();
            LoadPlayer();
            LoadCommands();
            LoadExit();
        }

        private void LoadExit()
        {
            var offset = new Point(192, 0);
            exit = new TriggerCollidable(new Rectangle(DungeonConstants.DungeonPosition + offset, new Point(64, 64)));
            collisionController.AddCollidable(exit);

            exit.OnTrigger += Exit;
        }

        private void Exit(Direction direction)
        {
            exit.OnTrigger -= Exit;
            commandManager.Execute(CommandType.RoomTransitionCommand, 
                new object[] { SceneManager.MARIO_ENTRANCE_ROOM, direction });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
