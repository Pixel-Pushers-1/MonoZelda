using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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
            {new (Keys.Enter, true), CommandType.StartGameCommand},
            {new (Keys.W, false), CommandType.PlayerMoveCommand},
            {new (Keys.Up, false), CommandType.PlayerMoveCommand},
            {new (Keys.S, false), CommandType.PlayerMoveCommand},
            {new (Keys.Down, false), CommandType.PlayerMoveCommand},
            {new (Keys.D, false), CommandType.PlayerMoveCommand},
            {new (Keys.Right, false), CommandType.PlayerMoveCommand},
            {new (Keys.A, false), CommandType.PlayerMoveCommand},
            {new (Keys.Left, false), CommandType.PlayerMoveCommand},
            {new (Keys.G, true), CommandType.ToggleGizmosCommand},
            {new (Keys.D1, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D2, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D3, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D4, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D5, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D6, true), CommandType.PlayerUseItemCommand},
            {new (Keys.T, true), CommandType.PlayerFireSwordBeam},
            {new (Keys.Z, true), CommandType.PlayerAttackCommand},
            {new (Keys.N, true), CommandType.PlayerAttackCommand},
            {new (Keys.Q, false), CommandType.ExitCommand},
            {new (Keys.R, false), CommandType.ResetCommand},
            {new (Keys.None, false), CommandType.PlayerStandingCommand},
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

            if (_directionalKeys.Contains(key))
                continue;

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