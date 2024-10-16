using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public interface ICommand
{
    void Execute(Keys PressedKey);
    void UnExecute();
}
