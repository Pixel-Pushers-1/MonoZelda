using System;
using System.Diagnostics;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerAttackCommand : ICommand
{
    private IController controller;
    private int attackIdx;
    private Player player; 

    public PlayerAttackCommand()
    {

    }

    public PlayerAttackCommand(Player player)
    {
        this.player = player;
    }

    private void SetAttackIndex(Keys PressedKey)
    {
        if(PressedKey == Keys.Z)
        {
            attackIdx = 0;
        }
        else if (PressedKey == Keys.N)
        {
            attackIdx = 1;
        }
    }

    public GameState Execute(Keys PressedKey)
    {
        // SetAttackIndex
        SetAttackIndex(PressedKey);

        if (player != null)
        {
            player.AttackingPlayer();
        }

        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }
}
