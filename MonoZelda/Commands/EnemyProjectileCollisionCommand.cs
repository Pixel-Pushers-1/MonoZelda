namespace PixelPushers.MonoZelda.Commands;

public class EnemyProjectileCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyProjectileCollisionCommand()
    {
        //empty
    }

    public EnemyProjectileCollisionCommand(MonoZeldaGame game)
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
