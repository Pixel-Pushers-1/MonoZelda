using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands.GameCommands;

public class PlayerUseEquippableCommand : ICommand
{
    private PlayerSpriteManager player;
    private EquippableManager equippableManager;

    public PlayerUseEquippableCommand()
    {
        //empty
    }

    public PlayerUseEquippableCommand(EquippableManager equippableManager, PlayerSpriteManager player)
    {
        this.player = player;
        this.equippableManager = equippableManager;
    }

    public void Execute(params object[] metadata)
    {
        player?.UseItem();
        equippableManager?.UseEquippedItem();
    }

    public void UnExecute()
    {
        //empty
    }
}
