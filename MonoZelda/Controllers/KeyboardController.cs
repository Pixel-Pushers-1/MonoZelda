using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using System.Collections.Generic;

namespace MonoZelda.Controllers;

public class KeyboardController : IController
{
    private CommandManager _commandManager;
    private Dictionary<(Keys key,bool oneShot), CommandType> _keyCommandDictionary;

    public KeyboardController(CommandManager commandManager)
    {
        _commandManager = commandManager;
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
            {new (Keys.D1, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D2, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D3, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D4, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D5, true), CommandType.PlayerUseItemCommand},
            {new (Keys.D6, true), CommandType.PlayerUseItemCommand},
            {new (Keys.Z, true), CommandType.PlayerAttackCommand},
            {new (Keys.N, true), CommandType.PlayerAttackCommand},
            {new (Keys.Q, false), CommandType.ExitCommand},
            {new (Keys.R, false), CommandType.ResetCommand},
            {new (Keys.None, false), CommandType.PlayerStandingCommand},
        };
    }

    // Properties
    public KeyboardState CurrentKeyboardState { get; private set; }
    public KeyboardState PreviousKeyboardState { get; private set; }

    public void Update(GameTime gameTime)
    {
        CurrentKeyboardState = Keyboard.GetState();

        // Iterate keyCommandDictionary to check input
        foreach (var keyCommandPair in _keyCommandDictionary)
        {
            (Keys key, bool oneShot) keyOneShot = keyCommandPair.Key;
            if (!keyOneShot.oneShot && (CurrentKeyboardState.IsKeyDown(keyOneShot.key) || keyOneShot.key == Keys.None))
            {
                _commandManager.Execute(keyCommandPair.Value, keyOneShot.key);
                break;
            }
            else if(keyOneShot.oneShot && OneShotPressed(keyOneShot.key))
            {
                _commandManager.Execute(keyCommandPair.Value, keyOneShot.key);
                break;
            }
        }

        // Update previous keyboard state (Do after all keyboard checks)
        PreviousKeyboardState = CurrentKeyboardState;
    }

    private bool OneShotPressed(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
    }

}

