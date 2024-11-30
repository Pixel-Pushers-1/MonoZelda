using MonoZelda.Dungeons;
using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    public class EnterDungeonAnimationCommand : ICommand
    {
        private SceneManager sceneManager;

        public EnterDungeonAnimationCommand() { }

        public EnterDungeonAnimationCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void Execute(params object[] metadata)
        {
            IDungeonRoom room = (IDungeonRoom)metadata[0];
            sceneManager.EnterDungeonScene(room);
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}

