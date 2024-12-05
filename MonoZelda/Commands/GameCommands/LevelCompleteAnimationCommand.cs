using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    internal class LevelCompleteAnimationCommand : ICommand
    {
        private SceneManager sceneManager;

        public LevelCompleteAnimationCommand() { }

        public LevelCompleteAnimationCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void Execute(params object[] metadata)
        {
            sceneManager.LevelCompleteScene();
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
