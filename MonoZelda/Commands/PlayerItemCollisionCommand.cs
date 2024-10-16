namespace MonoZelda.Commands;

public class PlayerItemCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerItemCollisionCommand()
    {
        //empty
    }

    public PlayerItemCollisionCommand(MonoZeldaGame game)
    {
        this.game = game;
    }

    public void Execute(params object[] metadata)
    {
        // ADD EXECUTE LOGIC HERE
    }

    public void UnExecute()
    {
        //empty
    }
}
