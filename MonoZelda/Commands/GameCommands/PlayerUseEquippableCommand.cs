using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerUseEquippableCommand : ICommand
{
    private PlayerSpriteManager player;

    public PlayerUseEquippableCommand()
    {
        //empty
    }

    public PlayerUseEquippableCommand(PlayerSpriteManager player)
    {
        this.player = player;
    }

    public void Execute(params object[] metadata)
    {
        player?.UseItem();
        PlayerState.EquippableManager?.UseEquippedItem();
    }

    public void UnExecute()
    {
        //empty
    }
}
