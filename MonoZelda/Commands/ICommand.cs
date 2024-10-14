using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public interface ICommand
{
    MonoZeldaGame Game {get; set;}

    void Execute(Keys PressedKey);
    void UnExecute();
}
