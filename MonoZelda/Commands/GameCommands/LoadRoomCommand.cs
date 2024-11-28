using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    internal class LoadRoomCommand : ICommand
    {
        private DungeonSceneManager dungeonScene;

        public LoadRoomCommand() { }

        public LoadRoomCommand(DungeonSceneManager dungeonScene)
        {
            this.dungeonScene = dungeonScene;
        }

        public void Execute(params object[] metadata)
        {
            if (dungeonScene == null)
            {
                return;
            }

            var destination = metadata[0];
            if (destination is string dest && !string.IsNullOrEmpty(dest))
            {
                dungeonScene.LoadRoom(dest);
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
