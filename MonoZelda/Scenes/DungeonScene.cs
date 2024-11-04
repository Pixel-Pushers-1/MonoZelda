using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Commands;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System;

namespace MonoZelda.Scenes
{
    public class DungeonScene : Scene
    {
        private IDungeonRoomLoader roomManager;
        private CollisionController collisionController;
        private GraphicsDevice graphicsDevice;
        private CommandManager commandManager;
        private ContentManager contentManager;

        private InventoryScene inventoryScene;
        private IDungeonRoom currentRoom;

        public string StartRoom { get; private set; }

        private IScene activeScene;

        public DungeonScene(string startRoom, GraphicsDevice graphicsDevice, CommandManager commandManager)
        {
            this.graphicsDevice = graphicsDevice;
            roomManager = new DungeonManager();
            collisionController = new CollisionController(commandManager);
            StartRoom = startRoom;
            this.commandManager = commandManager;

            inventoryScene = new InventoryScene(graphicsDevice, commandManager);

            commandManager.ReplaceCommand(CommandType.LoadRoomCommand, new LoadRoomCommand(this));
            commandManager.ReplaceCommand(CommandType.RoomTransitionCommand, new RoomTransitionCommand(this));
        }

        public void TransitionRoom(string roomName,Direction doorCollisionDirection)
        {
            resetScene();

            var nextRoom = roomManager.LoadRoom(roomName);
            var command = commandManager.GetCommand(CommandType.LoadRoomCommand);

            activeScene = new TransitionScene(currentRoom, nextRoom, command, doorCollisionDirection);
            activeScene.LoadContent(contentManager);
        }

        public void LoadRoom(string roomName)
        {
            resetScene();

            currentRoom = roomManager.LoadRoom(roomName);

            // TODO: Check for the mario level
            if (roomName == "mario")
            {
                activeScene = new MarioLevelScene();
                activeScene.LoadContent(contentManager);
                return;
            }

            activeScene = new RoomScene(graphicsDevice, commandManager, collisionController, currentRoom);
            activeScene.LoadContent(contentManager);
        }

        public override void Draw(SpriteBatch batch)
        {
            inventoryScene.Draw(batch);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            inventoryScene.LoadContent(contentManager);

            // We begin by revealing the the first room
            var room = roomManager.LoadRoom(StartRoom);
            activeScene = new EnterDungeonScene(this, room, graphicsDevice);
            activeScene.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            collisionController.Update(gameTime);

            activeScene.Update(gameTime);
            inventoryScene.Update(gameTime);
        }

        private void resetScene()
        {
            collisionController.Clear();
            SpriteDrawer.Reset();
        }
    }
}
