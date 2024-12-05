using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    internal class LinkDeathAnimationCommand : ICommand
    {
        private SceneManager sceneManager;

        public LinkDeathAnimationCommand() { }

        public LinkDeathAnimationCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void Execute(params object[] metadata)
        {
            sceneManager.LinkDeathScene();
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
