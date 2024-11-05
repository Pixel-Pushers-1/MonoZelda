using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerTakeDamageCommand : ICommand
{
    private PlayerState player;

    public PlayerTakeDamageCommand()
    {
        //empty
    }

    public PlayerTakeDamageCommand(PlayerState player)
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
