using Microsoft.Xna.Framework.Input;
using MonoZelda.Dungeons;
using MonoZelda.Link;
using MonoZelda.Scenes;
using MonoZelda.UI;

namespace MonoZelda.Commands.GameCommands
{
    public class RoomTransitionCommand : ICommand
    {
        private SceneManager sceneManager;

        public RoomTransitionCommand() { }

        public RoomTransitionCommand(SceneManager sceneManager)
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
            Direction collisionDirection = (Direction)metadata[1];
            if (destination is string dest && !string.IsNullOrEmpty(dest))
            {
                sceneManager.TransitionRoom(dest,collisionDirection);
            }
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
