﻿using System;
using PixelPushers.MonoZelda.Link;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;

public class PlayerAttackCommand : ICommand
{
    private int attackIdx;
    private Player player; 

    public MonoZeldaGame _game { get; set; }

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

    public void Execute(Keys PressedKey)
    {
        // SetAttackIndex
        SetAttackIndex(PressedKey);

        if (player != null)
        {
            player.AttackingPlayer();
        }
    }

    public void UnExecute()
    {
        throw new NotImplementedException();
    }
}
