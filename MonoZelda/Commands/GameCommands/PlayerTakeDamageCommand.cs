using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerTakeDamageCommand : ICommand
{
    private PlayerState player;
    private PlayerSpriteManager playerSprite;

    public PlayerTakeDamageCommand()
    {
        //empty
    }

    public PlayerTakeDamageCommand(PlayerState player, PlayerSpriteManager playerSprite)
    {
        this.player = player;
        this.playerSprite = playerSprite;
    }

    public void Execute(params object[] metadata)
    {
        if (player == null)
            return;

        // Apply damage to player
        player.TakeDamage();
        playerSprite.TakeDamage();
    }

    public void UnExecute()
    {
        //empty
    }
}
