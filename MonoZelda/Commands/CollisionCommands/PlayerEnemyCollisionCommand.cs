namespace MonoZelda.Commands.CollisionCommands;

public class PlayerEnemyCollisionCommand : ICommand
{
    private MonoZeldaGame game;

    public PlayerEnemyCollisionCommand()
    {
        //empty
    }

    public PlayerEnemyCollisionCommand(MonoZeldaGame game)
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
