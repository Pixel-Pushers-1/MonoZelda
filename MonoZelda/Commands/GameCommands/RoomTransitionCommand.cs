using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Scenes;
using MonoZelda.UI;

namespace MonoZelda.Commands.GameCommands
{
    public class RoomTransitionCommand : ICommand
    {
        private DungeonSceneManager dungeonScene;

        public RoomTransitionCommand() { }

        public RoomTransitionCommand(DungeonSceneManager dungeonScene)
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
            Direction collisionDirection = (Direction)metadata[1];
            if (destination is string dest && !string.IsNullOrEmpty(dest))
            {
                dungeonScene.TransitionRoom(dest,collisionDirection);
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
