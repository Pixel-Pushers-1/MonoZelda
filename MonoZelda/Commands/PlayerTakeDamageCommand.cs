using System;
using System.Diagnostics;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerTakeDamageCommand : ICommand
{
    IController controller;
    int damage;
    private Player player;
    public PlayerTakeDamageCommand()
    {
    }

    public PlayerTakeDamageCommand(Player player)
    {
        this.player = player;
    }

    public GameState Execute(Keys PressedKey)
    {
        if (player == null)
            return controller.GameState;

        // Apply damage to player
        player.PlayerTakeDamage();
        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetDamage(int damage) 
    {
        this.damage = damage;
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }
}
