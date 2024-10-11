using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;
using System;
using System.Collections.Generic;

namespace PixelPushers.MonoZelda.Controllers;

public class KeyboardController : IController
{
    private KeyboardState previousKeyboardState;
    private KeyboardState currentKeyboardState;
    private GameState gameState;
    private CommandManager commandManager;
    private Dictionary<Tuple<Keys,OneShot>,CommandEnum> keyCommandDictionary;

    public KeyboardController(CommandManager commandManager)
    {
        gameState = GameState.Title;
        this.commandManager = commandManager;
        keyCommandDictionary = new Dictionary<Tuple<Keys, OneShot>, CommandEnum>
        {
            {Tuple.Create(Keys.Enter,OneShot.YES), CommandEnum.StartGameCommand},
            {Tuple.Create(Keys.W,OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.Up,OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.S,OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.Down, OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.D, OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.Right, OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.A, OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.Left, OneShot.NO),CommandEnum.PlayerMoveCommand},
            {Tuple.Create(Keys.D1, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.D2, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.D3, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.D4, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.D5, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.D6, OneShot.YES),CommandEnum.PlayerUseItemCommand},
            {Tuple.Create(Keys.Z, OneShot.YES),CommandEnum.PlayerAttackCommand},
            {Tuple.Create(Keys.N, OneShot.YES),CommandEnum.PlayerAttackCommand},
            {Tuple.Create(Keys.Q, OneShot.NO),CommandEnum.ExitCommand},
            {Tuple.Create(Keys.R, OneShot.NO),CommandEnum.ResetCommand},
            {Tuple.Create(Keys.None, OneShot.NO),CommandEnum.PlayerStandingCommand},
        };
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
        currentKeyboardState = Keyboard.GetState();
        GameState newState = gameState;

        // Set controller for all commands
        commandManager.SetController(this);

        // Iterate keyCommandDictionary to check input
        foreach (var keyCommandPair in keyCommandDictionary)
        {
            Tuple<Keys, OneShot> keyOneShot = keyCommandPair.Key;
            if (keyOneShot.Item2 == OneShot.NO && (currentKeyboardState.IsKeyDown(keyOneShot.Item1) || keyOneShot.Item1 == Keys.None))
            {
                newState = commandManager.Execute(keyCommandPair.Value, keyOneShot.Item1);
                break;
            }
            else if(keyOneShot.Item2 == OneShot.YES && OneShotPressed(keyOneShot.Item1))
            {
                newState = commandManager.Execute(keyCommandPair.Value, keyOneShot.Item1);
                break;
            }
        }

        // Update previous keyboard state (Do after all keyboard checks)
        previousKeyboardState = currentKeyboardState;

        // Setting new Game State of keyboard controller if needed
        return (gameState != newState) ? (gameState = newState) == newState : false;
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

