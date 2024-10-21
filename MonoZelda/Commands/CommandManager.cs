using System.Collections.Generic;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Commands.CollisionCommands;

namespace MonoZelda.Commands;

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
    PlayerItemCollisionCommand,
    PlayerEnemyCollisionCommand,
    PlayerProjectileCollisionCommand,
    PlayerStaticCollisionCommand,
    PlayerTriggerCollisionCommand,
    EnemyProjectileCollisionCommand,
    EnemyStaticCollisionCommand,
    ToggleGizmosCommand,
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
        AddCommand(CommandType.PlayerItemCollisionCommand, new PlayerItemCollisionCommand());
        AddCommand(CommandType.PlayerEnemyCollisionCommand, new PlayerEnemyCollisionCommand());
        AddCommand(CommandType.PlayerProjectileCollisionCommand, new PlayerProjectileCollisionCommand());
        AddCommand(CommandType.PlayerStaticCollisionCommand, new PlayerStaticCollisionCommand());
        AddCommand(CommandType.PlayerTriggerCollisionCommand, new PlayerTriggerCollisionCommand());
        AddCommand(CommandType.EnemyProjectileCollisionCommand, new EnemyProjectileCollisionCommand());
        AddCommand(CommandType.EnemyStaticCollisionCommand, new EnemyStaticCollisionCommand());
        AddCommand(CommandType.ToggleGizmosCommand, new ToggleGizmosCommand());

    }

    public void Execute(CommandType commandType, params object[] metadata)
    {
        commandMap[commandType].Execute(metadata);
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