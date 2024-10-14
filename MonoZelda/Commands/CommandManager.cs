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
        commandMap = new Dictionary<CommandEnum, ICommand>();
        AddCommand(CommandEnum.ExitCommand, new ExitCommand());
        AddCommand(CommandEnum.PlayerAttackCommand, new PlayerAttackCommand());
        AddCommand(CommandEnum.PlayerMoveCommand, new PlayerMoveCommand());
        AddCommand(CommandEnum.PlayerUseItemCommand, new PlayerUseItemCommand());
        AddCommand(CommandEnum.PlayerStandingCommand, new PlayerStandingCommand());
        AddCommand(CommandEnum.ResetCommand, new ResetCommand());
        AddCommand(CommandEnum.StartGameCommand, new StartGameCommand());
        AddCommand(CommandEnum.LoadRoomCommand, new LoadRoomCommand());
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