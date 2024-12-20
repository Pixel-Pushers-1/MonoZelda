﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using System.Collections.Generic;
using System.Linq;

namespace MonoZelda.Controllers;

public class KeyboardController : IController
{
    private CommandManager _commandManager;
    private Dictionary<(Keys key, bool oneShot), CommandType> _keyCommandDictionary;
    private List<Keys> _pressedDirectionalKeys;
    private static readonly Keys[] _directionalKeys = new[]
    {
        Keys.W, Keys.Up,
        Keys.S, Keys.Down,
        Keys.A, Keys.Left,
        Keys.D, Keys.Right
    };

    public KeyboardController(CommandManager commandManager)
    {
        _commandManager = commandManager;
        _pressedDirectionalKeys = new List<Keys>();
        _keyCommandDictionary = new Dictionary<(Keys key, bool oneShot), CommandType>
        {
            {new (Keys.G, true), CommandType.ToggleGizmosCommand},
            {new (Keys.F, true), CommandType.PlayerUseEquippableCommand},
            {new (Keys.Z, true), CommandType.PlayerAttackCommand},
            {new (Keys.N, true), CommandType.PlayerAttackCommand},
            {new (Keys.M, true), CommandType.MuteCommand},
            {new (Keys.Q, false), CommandType.ExitCommand},
            {new (Keys.R, false), CommandType.ResetCommand},
            {new (Keys.None, false), CommandType.PlayerStandingCommand},
            {new (Keys.I, true), CommandType.ToggleInventoryCommand},
            {new (Keys.Escape, true), CommandType.ToggleInventoryCommand},
            {new (Keys.W, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.A, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.S, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.D, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.Up, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.Left, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.Down, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.Right, true), CommandType.NavigableGridMoveCommand},
            {new (Keys.Enter, true), CommandType.NavigableGridExecuteCommand},
            {new (Keys.F5, true), CommandType.QuickSaveCommand},
            {new (Keys.F9, true), CommandType.QuickLoadCommand},
        };
    }

    public KeyboardState CurrentKeyboardState { get; private set; }
    public KeyboardState PreviousKeyboardState { get; private set; }

    public void Update(GameTime gameTime)
    {
        CurrentKeyboardState = Keyboard.GetState();
        UpdateDirectionalKeysList();

        if (_pressedDirectionalKeys.Any())
        {
            Keys mostRecentKey = _pressedDirectionalKeys.Last();
            _commandManager.Execute(CommandType.PlayerMoveCommand, mostRecentKey);
        }
        else
        {
            _commandManager.Execute(CommandType.PlayerStandingCommand, Keys.None);
        }

        foreach (var keyCommandPair in _keyCommandDictionary)
        {
            (Keys key, bool oneShot) = keyCommandPair.Key;

            if (!oneShot && CurrentKeyboardState.IsKeyDown(key))
            {
                _commandManager.Execute(keyCommandPair.Value, key);
                break;
            }
            else if (oneShot && OneShotPressed(key))
            {
                _commandManager.Execute(keyCommandPair.Value, key);
                break;
            }
        }

        PreviousKeyboardState = CurrentKeyboardState;
    }

    private void UpdateDirectionalKeysList()
    {
        foreach (Keys key in _directionalKeys)
        {
            bool isCurrentlyPressed = CurrentKeyboardState.IsKeyDown(key);
            bool wasPressed = PreviousKeyboardState.IsKeyDown(key);

            if (isCurrentlyPressed && !wasPressed)
            {
                _pressedDirectionalKeys.Remove(key);
                _pressedDirectionalKeys.Add(key);
            }
            else if (!isCurrentlyPressed && wasPressed)
            {
                _pressedDirectionalKeys.Remove(key);
            }
        }
    }
    private bool OneShotPressed(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
    }
}