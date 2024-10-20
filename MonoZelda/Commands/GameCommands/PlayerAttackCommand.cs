﻿using MonoZelda.Link;

namespace MonoZelda.Commands.GameCommands;

public class PlayerAttackCommand : ICommand
{
    private Player player;

    public PlayerAttackCommand()
    {
        //empty
    }

    public PlayerAttackCommand(Player player)
    {
        this.player = player;
    }

    public void Execute(params object[] metadata)
    {
        player?.Attack();
    }

    public void UnExecute()
    {
        //empty
    }
}