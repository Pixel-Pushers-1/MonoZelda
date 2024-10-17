using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;

namespace MonoZelda.Commands
{
    internal class LoadRoomCommand : ICommand
    {
        private MonoZeldaGame game;
        private IDungeonRoom room;

        public LoadRoomCommand() { }

        public LoadRoomCommand(MonoZeldaGame game, IDungeonRoom room)
        {
            this.game = game;
            this.room = room;
        }

        public void Execute(params object[] metadata)
        {
            MouseState mouseState = (MouseState)metadata[0];
            if (room == null || game == null)
            {
                return;
            }

            // TODO: Logic in the command is bad but this is temporary
            var doors = room.GetDoors();

            // CommandManager dosn't expose a method to get commands, so we can't assign this in the mouse controller

            foreach (var door in doors)
            {
                if (door.Bounds.Contains(mouseState.X, mouseState.Y) && !string.IsNullOrEmpty(door.Destination))
                {
                    game.LoadDungeon(door.Destination);
                    return;
                }
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
