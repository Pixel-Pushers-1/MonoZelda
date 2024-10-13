using System;
using PixelPushers.MonoZelda.Link;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerTakeDamageCommand : ICommand
{
    int damage;
    private Player player;

    public MonoZeldaGame _game { get; set; }
    public PlayerTakeDamageCommand()
    {
    }

    public PlayerTakeDamageCommand(Player player)
    {
        this.player = player;
    }

    public void Execute(Keys PressedKey)
    {
        if (player == null)
            return;

        // Apply damage to player
        player.PlayerTakeDamage();
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetDamage(int damage) 
    {
        this.damage = damage;
    }
}
