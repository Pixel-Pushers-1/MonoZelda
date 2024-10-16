using MonoZelda.Link;

namespace MonoZelda.Commands;

public class PlayerTakeDamageCommand : ICommand
{
    private Player player;

    public PlayerTakeDamageCommand()
    {
        //empty
    }

    public PlayerTakeDamageCommand(Player player)
    {
        this.player = player;
    }

    public void Execute(params object[] metadata)
    {
        if (player == null)
            return;

        // Apply damage to player
        player.TakeDamage();
    }

    public void UnExecute()
    {
        //empty
    }
}
