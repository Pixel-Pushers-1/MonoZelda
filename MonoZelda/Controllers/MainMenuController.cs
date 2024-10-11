using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;

namespace PixelPushers.MonoZelda.Controllers
{
    internal class MainMenuController : IController
    {
        public GameState GameState { get; set; }
        private CommandManager _commandManager;

        public MainMenuController(CommandManager commandManager)
        {
            _commandManager = commandManager;
            GameState = GameState.Start;
        }

        public bool Update()
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            var action = CommandEnum.None;
            foreach (var key in keys)
            {
                switch (key)
                {
                    case Keys.Enter:
                        action = CommandEnum.ExitCommand;
                        break;
                    default:
                        break;
                }
            }

            if (action != CommandEnum.None)
            {
                _commandManager.Execute(action,Keys.Enter);
            }

            return true;
        }
    }
}
