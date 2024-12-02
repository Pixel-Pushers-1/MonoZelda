using Microsoft.Xna.Framework.Input;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private PlayerSpriteManager player;
    private EquippableManager equippableManager;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(EquippableManager equippableManager, PlayerSpriteManager player)
    {
        this.player = player;
        this.equippableManager = equippableManager;
    }

    public void Execute(params object[] metadata)
    {
        player?.Attack();
        equippableManager?.UseSwordEquippable();
    }

    public void UnExecute()
    {
        //empty
    }
}
