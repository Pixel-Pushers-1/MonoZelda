using Microsoft.Xna.Framework.Input;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private PlayerSpriteManager player;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(PlayerSpriteManager player)
    {
        this.player = player;
    }

    public void Execute(params object[] metadata)
    {
        player?.Attack();
        PlayerState.EquippableManager?.UseSwordEquippable();
    }

    public void UnExecute()
    {
        //empty
    }
}
