using MonoZelda.Link;
using MonoZelda.Sound;

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
        // Play damage sound
        SoundManager.PlaySound("LOZ_Link_Hurt", false);

        // Apply damage to player
        PlayerState.TakeDamage();
        playerSprite.TakeDamage();
    }

    public void UnExecute()
    {
        //empty
    }
}
