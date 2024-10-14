using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;

namespace PixelPushers.MonoZelda.Controllers
{
    internal class MainMenuController : IController
    {
        private CommandManager _commandManager;

        public MainMenuController(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        public void Update(GameTime gameTime)
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
        }
    }
}
