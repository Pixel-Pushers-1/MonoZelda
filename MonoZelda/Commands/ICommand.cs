using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public interface ICommand
{
    void Execute(params object[] metadata);
    void UnExecute();
}
