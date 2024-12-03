using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    internal class LoadRoomCommand : ICommand
    {
        private SceneManager sceneManager;

        public LoadRoomCommand() { }

        public LoadRoomCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void Execute(params object[] metadata)
        {
            if (sceneManager == null)
            {
                return;
            }

            var destination = metadata[0];
            if (destination is string dest && !string.IsNullOrEmpty(dest))
            {
                sceneManager.LoadRoom(dest);
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
