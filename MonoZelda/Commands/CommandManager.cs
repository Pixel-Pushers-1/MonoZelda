using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Commands;

public enum CommandType
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
    Dictionary<CommandType, ICommand> commandMap;
    public CommandManager()
    {
        commandMap = new Dictionary<CommandType, ICommand>();
        AddCommand(CommandType.ExitCommand, new ExitCommand());
        AddCommand(CommandType.PlayerAttackCommand, new PlayerAttackCommand());
        AddCommand(CommandType.PlayerMoveCommand, new PlayerMoveCommand());
        AddCommand(CommandType.PlayerUseItemCommand, new PlayerUseItemCommand());
        AddCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand());
        AddCommand(CommandType.ResetCommand, new ResetCommand());
        AddCommand(CommandType.StartGameCommand, new StartGameCommand());
        AddCommand(CommandType.LoadRoomCommand, new LoadRoomCommand());
    }

    public void Execute(CommandType commandType, Keys PressedKey)
    {
        commandMap[commandType].Execute(PressedKey);
    }

    public bool ReplaceCommand(CommandType commandType, ICommand command)
    {
        if (commandMap.ContainsKey(commandType))
        {
            commandMap[commandType] = command;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool AddCommand(CommandType commandName, ICommand command)
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