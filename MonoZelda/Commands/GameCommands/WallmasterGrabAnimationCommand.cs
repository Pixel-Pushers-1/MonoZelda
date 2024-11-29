using MonoZelda.Scenes;

namespace MonoZelda.Commands.GameCommands
{
    internal class WallMasterGrabAnimationCommand : ICommand
    {
        private SceneManager sceneManager;

        public WallMasterGrabAnimationCommand() { }

        public WallMasterGrabAnimationCommand(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void Execute(params object[] metadata)
        {
            sceneManager.WallMasterGrabScene();
        }

        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}
