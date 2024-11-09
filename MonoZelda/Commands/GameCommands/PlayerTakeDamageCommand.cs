using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerTakeDamageCommand : ICommand
{
    private PlayerSpriteManager playerSprite;

    public PlayerTakeDamageCommand()
    {
        //empty
    }

    public PlayerTakeDamageCommand(PlayerSpriteManager playerSprite)
    {
        this.playerSprite = playerSprite;
    }

    public void Execute(params object[] metadata)
    {
        // Apply damage to player
        PlayerState.TakeDamage();
        playerSprite.TakeDamage();
    }

    public void UnExecute()
    {
        //empty
    }
}
