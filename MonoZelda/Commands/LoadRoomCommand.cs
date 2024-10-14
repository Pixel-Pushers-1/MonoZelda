using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using PixelPushers.MonoZelda;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Controllers;
using System.Numerics;

namespace MonoZelda.Commands
{
    internal class LoadRoomCommand
    {
        private MonoZeldaGame game;
        private IDungeonRoom room;

        public MonoZeldaGame _game { get; set; }
        public MouseState MouseState {get;set;}

        public LoadRoomCommand() { }

        public LoadRoomCommand(MonoZeldaGame game, IDungeonRoom room)
        {
            this.game = game;
            this.room = room;
        }

        public GameState Execute(Keys PressedKey)
        {
            if(room == null || game == null)
            {
                return GameState.None;
            }

            // TODO: Logic in the command is bad but this is temporary
            var doors = room.GetDoors();

            // CommandManager dosn't expose a method to get commands, so we can't assign this in the mouse controller
            MouseState = Mouse.GetState();

            foreach (var door in doors)
            {
                if (door.Bounds.Contains(MouseState.X, MouseState.Y) && !string.IsNullOrEmpty(door.Destination))
                {
                    game.LoadDungeon(door.Destination);
                    return GameState.None;
                }
            }

            return GameState.None;
        }

        public void SetController(IController controller)
        {
        }

        public GameState UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
