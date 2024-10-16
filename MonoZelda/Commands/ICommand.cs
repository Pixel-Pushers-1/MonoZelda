namespace MonoZelda.Commands;

public interface ICommand
{
    void Execute(params object[] metadata);
    void UnExecute();
}
