using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    public class RoomTransitionCommand : ICommand
    {
        private DungeonScene dungeonScene;

        public RoomTransitionCommand() { }

        public RoomTransitionCommand(DungeonScene dungeonScene)
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
                dungeonScene.TransitionRoom(dest);
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
