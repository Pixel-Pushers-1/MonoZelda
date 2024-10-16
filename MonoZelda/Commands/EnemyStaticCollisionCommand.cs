namespace PixelPushers.MonoZelda.Commands;

public class EnemyStaticCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public EnemyStaticCollisionCommand()
    {
        //empty
    }

    public EnemyStaticCollisionCommand(MonoZeldaGame game)
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
