using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public interface ICommand
{
    MonoZeldaGame _game {get; set;}

    void Execute(Keys PressedKey);
    void UnExecute();
}
