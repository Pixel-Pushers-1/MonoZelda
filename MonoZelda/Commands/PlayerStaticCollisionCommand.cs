namespace PixelPushers.MonoZelda.Commands;

public class PlayerStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerStaticCollisionCommand()
    {
        //empty
    }

    public PlayerStaticCollisionCommand(MonoZeldaGame game)
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
