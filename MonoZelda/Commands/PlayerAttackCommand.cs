﻿using System;
using System.Diagnostics;
using MonoZelda.Player;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerAttackCommand : ICommand
{
    IController controller;
    int attackIdx;
    private Player player; 

    public PlayerAttackCommand()
    {

    }

    public PlayerAttackCommand(Player player)
    {
        this.player = player; 
    }

    public GameState Execute()
    {
        Debug.WriteLine("Player using attack " + attackIdx);
        player.AttackingPlayer(this);
        // Keep GameState the same inside the controller
        return controller.GameState;
    }

    public GameState UnExecute()
    {
        throw new NotImplementedException();
    }

    public void SetAttackIndex(int attackIdx)
    {
        this.attackIdx = attackIdx;
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }
}
