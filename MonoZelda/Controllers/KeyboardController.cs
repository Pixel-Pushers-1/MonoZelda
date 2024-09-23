﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;
using PixelPushers.MonoZelda.Items;

namespace PixelPushers.MonoZelda.Controllers;

public class KeyboardController : IController
{
    private KeyboardState previousKeyboardState;
    private KeyboardState currentKeyboardState;
    private GameState gameState;
    private CommandManager commandManager;
    private Item GameItems;
    private ItemList previousItem;

    public KeyboardController(CommandManager commandManager)
    {
        gameState = GameState.Start;
        this.commandManager = commandManager;
        this.GameItems = new Item();
        previousItem = ItemList.None;
    }

    // Properties
    public KeyboardState CurrentKeyboardState
    {
        get
        {
            return currentKeyboardState;
        }
        set
        {
            currentKeyboardState = value;
        }
    }

    public KeyboardState PreviousKeyboardState
    {
        get
        {
            return previousKeyboardState;
        }
        set
        {
            previousKeyboardState = value;
        }
    }

    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
        }
    }


    public bool Update()
    {
        commandManager.SetController(this);
        GameState newState = gameState;
        if (currentKeyboardState.IsKeyDown(Keys.Q))
        {
            // Exit Command
            commandManager.Execute(CommandEnum.ExitCommand);
        }
        else if (OneShotPressed(Keys.R))
        {
            // Reset Command
            commandManager.Execute(CommandEnum.ResetCommand);
        }
        else
        {
            // Check for Player item swap input
            if (OneShotPressed(Keys.D1))
            {
                // Player item 1 equip
                PlayerUseItemCommand playerUseItemCommand = (PlayerUseItemCommand) commandManager.CommandMap[CommandEnum.PlayerUseItemCommand];
                playerUseItemCommand.SetItemIndex(1);
                commandManager.Execute(CommandEnum.PlayerUseItemCommand);
            }
            else if (OneShotPressed(Keys.D2))
            {
                // Player item 2 equip
                PlayerUseItemCommand playerUseItemCommand = (PlayerUseItemCommand) commandManager.CommandMap[CommandEnum.PlayerUseItemCommand];
                playerUseItemCommand.SetItemIndex(2);
                commandManager.Execute(CommandEnum.PlayerUseItemCommand);
            }
            else if (OneShotPressed(Keys.D3))
            {
                // Player item 3 equip
                PlayerUseItemCommand playerUseItemCommand = (PlayerUseItemCommand) commandManager.CommandMap[CommandEnum.PlayerUseItemCommand];
                playerUseItemCommand.SetItemIndex(3);
                commandManager.Execute(CommandEnum.PlayerUseItemCommand);
            }
            else if (OneShotPressed(Keys.D4))
            {
                // Player item 4 equip
                PlayerUseItemCommand playerUseItemCommand = (PlayerUseItemCommand) commandManager.CommandMap[CommandEnum.PlayerUseItemCommand];
                playerUseItemCommand.SetItemIndex(4);
                commandManager.Execute(CommandEnum.PlayerUseItemCommand);
            }

            // Check for Player movement input
            if (currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.Up))
            {
                // Player move forward command
                PlayerMoveCommand playerMoveCommand = (PlayerMoveCommand) commandManager.CommandMap[CommandEnum.PlayerMoveCommand];
                playerMoveCommand.SetScalarVector(new Vector2(1, 0));
                commandManager.Execute(CommandEnum.PlayerMoveCommand);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down))
            {
                // Player move backward command
                PlayerMoveCommand playerMoveCommand = (PlayerMoveCommand) commandManager.CommandMap[CommandEnum.PlayerMoveCommand];
                playerMoveCommand.SetScalarVector(new Vector2(-1, 0));
                commandManager.Execute(CommandEnum.PlayerMoveCommand);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right))
            {
                // Player move right command
                PlayerMoveCommand playerMoveCommand = (PlayerMoveCommand) commandManager.CommandMap[CommandEnum.PlayerMoveCommand];
                playerMoveCommand.SetScalarVector(new Vector2(0, 1));
                commandManager.Execute(CommandEnum.PlayerMoveCommand);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left))
            {
                // Player move left command
                PlayerMoveCommand playerMoveCommand = (PlayerMoveCommand) commandManager.CommandMap[CommandEnum.PlayerMoveCommand];
                playerMoveCommand.SetScalarVector(new Vector2(0, -1));
                commandManager.Execute(CommandEnum.PlayerMoveCommand);
            }

            // Check for Player attack input
            if (OneShotPressed(Keys.Z))
            {
                // Player primary attack
                PlayerAttackCommand playerAttackCommand = (PlayerAttackCommand) commandManager.CommandMap[CommandEnum.PlayerAttackCommand];
                playerAttackCommand.SetAttackIndex(1);
                commandManager.Execute(CommandEnum.PlayerAttackCommand);
            }
            else if (OneShotPressed(Keys.N))
            {
                // Player primary attack
                PlayerAttackCommand playerAttackCommand = (PlayerAttackCommand) commandManager.CommandMap[CommandEnum.PlayerAttackCommand];
                playerAttackCommand.SetAttackIndex(2);
                commandManager.Execute(CommandEnum.PlayerAttackCommand);
            }

            // Check for Player damage applied
            if (OneShotPressed(Keys.E))
            {
                PlayerTakeDamageCommand playerTakeDamageCommand = (PlayerTakeDamageCommand) commandManager.CommandMap[CommandEnum.PlayerTakeDamageCommand];
                playerTakeDamageCommand.SetDamage(10);
                commandManager.Execute(CommandEnum.PlayerTakeDamageCommand);
            }

            // Check for Block / Obstacle cycle input
            if (OneShotPressed(Keys.Y))
            {
                BlockCycleCommand blockCycleCommand = (BlockCycleCommand) commandManager.CommandMap[CommandEnum.BlockCycleCommand];
                blockCycleCommand.SetCycleAddition(1);
                commandManager.Execute(CommandEnum.BlockCycleCommand);
            }
            else if (OneShotPressed(Keys.T))
            {
                BlockCycleCommand blockCycleCommand = (BlockCycleCommand) commandManager.CommandMap[CommandEnum.BlockCycleCommand];
                blockCycleCommand.SetCycleAddition(-1);
                commandManager.Execute(CommandEnum.BlockCycleCommand);
            }

            // Check for Item cycle input
            if (OneShotPressed(Keys.I))
            {
                ItemCycleCommand itemCycleCommand = (ItemCycleCommand) commandManager.CommandMap[CommandEnum.ItemCycleCommand];
                itemCycleCommand.SetController(this);
                itemCycleCommand.SetItemObject(GameItems);
                previousItem = GameItems.CurrentItem;
                if (previousItem == ItemList.BluePotion)
                {
                    itemCycleCommand.SetCycleAddition(-14);
                }
                else
                {
                    itemCycleCommand.SetCycleAddition(1);

                }
                System.Diagnostics.Debug.WriteLine("Current Item is: " + GameItems.CurrentItem);
                commandManager.Execute(CommandEnum.ItemCycleCommand);
            }
            else if (OneShotPressed(Keys.U))
            {
                ItemCycleCommand itemCycleCommand = (ItemCycleCommand) commandManager.CommandMap[CommandEnum.ItemCycleCommand];
                itemCycleCommand.SetController(this);
                itemCycleCommand.SetItemObject(GameItems);
                previousItem = GameItems.CurrentItem;
                if (previousItem == ItemList.Compass)
                {
                    itemCycleCommand.SetCycleAddition(14);
                }
                else
                {
                    itemCycleCommand.SetCycleAddition(-1);

                }
                System.Diagnostics.Debug.WriteLine("Current Item is: " + GameItems.CurrentItem);
                commandManager.Execute(CommandEnum.ItemCycleCommand);
            }

            // Check for Enemy / NPC cycle input
            if (OneShotPressed(Keys.P))
            {
                EnemyCycleCommand enemyCycleCommand = (EnemyCycleCommand) commandManager.CommandMap[CommandEnum.EnemyCycleCommand];
                enemyCycleCommand.SetCycleAddition(1);
                commandManager.Execute(CommandEnum.EnemyCycleCommand);
            }
            else if (OneShotPressed(Keys.O))
            {
                EnemyCycleCommand enemyCycleCommand = (EnemyCycleCommand) commandManager.CommandMap[CommandEnum.EnemyCycleCommand];
                enemyCycleCommand.SetCycleAddition(-1);
                commandManager.Execute(CommandEnum.EnemyCycleCommand);
            }
        }

        // Update previous keyboard state (Do after all keyboard checks)
        previousKeyboardState = currentKeyboardState;

        // Setting new Game State of keyboard controller if needed
        if (gameState != newState)
        {
            gameState = newState;
            return true;
        }
        return false;
    }

    public bool OneShotPressed(Keys key)
    {
        if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
        {
            return true;
        }
        return false;
}
}
