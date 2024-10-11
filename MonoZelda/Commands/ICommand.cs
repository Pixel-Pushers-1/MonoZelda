using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Commands;

public interface ICommand
{
    GameState Execute(Keys PressedKey);
    GameState UnExecute();
    void SetController(IController controller);
}
