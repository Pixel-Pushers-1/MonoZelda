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
    PlayerCycleEquippableCommand,
    PlayerUseEquippableCommand,
    ResetCommand,
    PlayerStandingCommand,
    StartGameCommand,
    LoadRoomCommand,
    RoomTransitionCommand,
    MuteCommand,
    PlayerItemCollisionCommand,
    PlayerEnemyCollisionCommand,
    PlayerEnemyProjectileCollisionCommand,
    PlayerStaticCollisionCommand,
    PlayerTriggerCollisionCommand,
    EnemyPlayerProjectileCollisionCommand,
    EnemyStaticRoomCollisionCommand,
    EnemyProjectileStaticBoundaryCollisionCommand,
    PlayerProjectileStaticRoomCollisionCommand,
    PlayerProjectileStaticBoundaryCollisionCommand,
    ToggleGizmosCommand,
    ToggleInventoryCommand,
    PlayerProjectileDoorCollisionCommand,
    PlayerDeathCommand,
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
        AddCommand(CommandType.PlayerUseEquippableCommand, new PlayerUseEquippableCommand());
        AddCommand(CommandType.PlayerCycleEquippableCommand, new PlayerCycleEquippableCommand());
        AddCommand(CommandType.PlayerStandingCommand, new PlayerStandingCommand());
        AddCommand(CommandType.ResetCommand, new ResetCommand());
        AddCommand(CommandType.StartGameCommand, new StartGameCommand());
        AddCommand(CommandType.LoadRoomCommand, new LoadRoomCommand());
        AddCommand(CommandType.RoomTransitionCommand, new RoomTransitionCommand());
        AddCommand(CommandType.MuteCommand, new MuteCommand());
        AddCommand(CommandType.PlayerItemCollisionCommand, new PlayerItemCollisionCommand());
        AddCommand(CommandType.PlayerEnemyCollisionCommand, new PlayerEnemyCollisionCommand());
        AddCommand(CommandType.PlayerEnemyProjectileCollisionCommand, new PlayerEnemyProjectileCollisionCommand());
        AddCommand(CommandType.PlayerStaticCollisionCommand, new PlayerStaticCollisionCommand());
        AddCommand(CommandType.PlayerTriggerCollisionCommand, new PlayerTriggerCollisionCommand());
        AddCommand(CommandType.EnemyPlayerProjectileCollisionCommand, new EnemyPlayerProjectileCollisionCommand());
        AddCommand(CommandType.EnemyStaticRoomCollisionCommand, new EnemyStaticRoomCollisionCommand());
        AddCommand(CommandType.EnemyProjectileStaticBoundaryCollisionCommand, new EnemyProjectileStaticBoundaryCollisionCommand());
        AddCommand(CommandType.PlayerProjectileStaticRoomCollisionCommand, new PlayerProjectileStaticRoomCollisionCommand());
        AddCommand(CommandType.PlayerProjectileStaticBoundaryCollisionCommand, new PlayerProjectileStaticBoundaryCollisionCommand());
        AddCommand(CommandType.ToggleGizmosCommand, new ToggleGizmosCommand());
        AddCommand(CommandType.ToggleInventoryCommand, new ToggleInventoryCommand());
        AddCommand(CommandType.PlayerDeathCommand, new PlayerDeathCommand());
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

    public ICommand GetCommand(CommandType commandType)
    {
        if(!commandMap.ContainsKey(commandType))
        {
            return default;
        }

        return commandMap[commandType];
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