using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Commands;

public enum OneShot
{
    YES,
    NO,
}

public enum CommandEnum
{
    // Commands for entire project
    ExitCommand,
    PlayerAttackCommand,
    PlayerMoveCommand,
    PlayerTakeDamageCommand,
    PlayerUseItemCommand,
    ResetCommand,
    PlayerStandingCommand,
    StartGameCommand,
    LoadRoomCommand,
    None
}

public class CommandManager
{
    Dictionary<CommandEnum, ICommand> commandMap;
    public CommandManager()
    {
        commandMap = new Dictionary<CommandEnum, ICommand>();
        AddCommand(CommandEnum.ExitCommand, new ExitCommand());
        AddCommand(CommandEnum.PlayerAttackCommand, new PlayerAttackCommand());
        AddCommand(CommandEnum.PlayerMoveCommand, new PlayerMoveCommand());
        AddCommand(CommandEnum.PlayerUseItemCommand, new PlayerUseItemCommand());
        AddCommand(CommandEnum.PlayerStandingCommand, new PlayerStandingCommand());
        AddCommand(CommandEnum.ResetCommand, new ResetCommand());
        AddCommand(CommandEnum.StartGameCommand, new StartGameCommand());
        //AddCommand(CommandEnum.LoadRoomCommand, new LoadRoomCommand());
    }

    public Dictionary<CommandEnum, ICommand> CommandMap
    {
        get
        {
            return commandMap;
        }
    }

    public void Execute(CommandEnum commandName,Keys PressedKey)
    {
        commandMap[commandName].Execute(PressedKey);
    }

    public bool ReplaceCommand(CommandEnum commandName, ICommand command)
    {
        if (commandMap.ContainsKey(commandName))
        {
            commandMap[commandName] = command;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddCommand(CommandEnum commandName, ICommand command)
    {
        if (commandMap.ContainsKey(commandName))
        {
            return false;
        }
        else
        {
            commandMap[commandName] = command;
            return true;
        }
    }
}