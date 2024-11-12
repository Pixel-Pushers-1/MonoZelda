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
using MonoZelda.Commands.CollisionCommands;
using MonoZelda.UI;

namespace MonoZelda.Scenes
{
    public class DungeonScene : Scene
    {
        public static readonly string MARIO_ROOM = "Room18";
        public static readonly string MARIO_ENTRANCE_ROOM = "Room17";
        
        private IDungeonRoomLoader roomManager;
        private CollisionController collisionController;
        private GraphicsDevice graphicsDevice;
        private CommandManager commandManager;
        private ContentManager contentManager;
        private InventoryScene inventoryScene;
        private IDungeonRoom currentRoom;
        private bool isPaused;

        public string StartRoom { get; private set; }

        private IScene activeScene;

        public DungeonScene(string startRoom, GraphicsDevice graphicsDevice, CommandManager commandManager)
        {
            this.graphicsDevice = graphicsDevice;
            roomManager = new DungeonManager();
            collisionController = new CollisionController(commandManager);
            StartRoom = startRoom;
            this.commandManager = commandManager;

            // Start the player near the entrance
            PlayerState.Initialize();

            // create inventory scene
            inventoryScene = new InventoryScene(graphicsDevice, commandManager);

            commandManager.ReplaceCommand(CommandType.LoadRoomCommand, new LoadRoomCommand(this));
            commandManager.ReplaceCommand(CommandType.RoomTransitionCommand, new RoomTransitionCommand(this));
            commandManager.ReplaceCommand(CommandType.ToggleInventoryCommand, new ToggleInventoryCommand(this));
            commandManager.ReplaceCommand(CommandType.PlayerEnemyCollisionCommand,
                new PlayerEnemyCollisionCommand(commandManager));
        }

        public void TransitionRoom(string roomName, Direction transitionDirection)
        {
            ResetScene();

            var nextRoom = roomManager.LoadRoom(roomName);
            var command = commandManager.GetCommand(CommandType.LoadRoomCommand);
            
            if (roomName == MARIO_ROOM || (roomName == MARIO_ENTRANCE_ROOM && currentRoom.RoomName == MARIO_ROOM))
            {
                activeScene = new MarioLevelTransitionScene(this, nextRoom, command, graphicsDevice);
            }
            else
            {
                activeScene = new TransitionScene(currentRoom, nextRoom, command, transitionDirection);
            }
            
            activeScene.LoadContent(contentManager);
        }

        public void LoadRoom(string roomName)
        {
            ResetScene();

            currentRoom = roomManager.LoadRoom(roomName);
            
            // Debugging purposes
            LevelTextWidget.LevelName = roomName;

            if (roomName == MARIO_ROOM)
            {
                activeScene = new MarioLevelScene(graphicsDevice, commandManager, collisionController, currentRoom);
            }
            else
            {
                activeScene = new RoomScene(graphicsDevice, commandManager, collisionController, currentRoom);
            }

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
            currentRoom = roomManager.LoadRoom("RoomTest");
            activeScene = new EnterDungeonScene(this, currentRoom, graphicsDevice);
            activeScene.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            inventoryScene.Update(gameTime);
            if (isPaused) return;

            collisionController.Update(gameTime);
            activeScene.Update(gameTime);
        }

        private void ResetScene()
        {
            collisionController.Clear();
            SpriteDrawer.Reset();
            //reset playerStateParams
            PlayerState.ResetCandle();
           
            activeScene.UnloadContent();
            
            // Complication due to SpriteDict getting cleared, need to re-init the UI
            inventoryScene.LoadContent(contentManager, currentRoom);
        }
        
        public void ToggleInventory()
        {
            isPaused = inventoryScene.ToggleInventory();
        }

        public void Pause()
        {
            isPaused = true;
        }
        
        public void UnPause()
        {
            isPaused = false;
        }
    }
}
