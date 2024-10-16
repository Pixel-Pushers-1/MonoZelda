namespace PixelPushers.MonoZelda.Commands;

public class PlayerProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerProjectileCollisionCommand()
    {
        //empty
    }

    public PlayerProjectileCollisionCommand(MonoZeldaGame game)
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
