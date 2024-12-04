using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using System;
using System.Collections.Generic;

namespace MonoZelda.Controllers
{
    public class GamepadController : IController
    {
        private readonly CommandManager _commandManager;
        private readonly Dictionary<(Buttons button, bool oneShot), CommandType> _buttonCommandDictionary;
        private GamePadState _currentState;
        private GamePadState _previousState;
        public PlayerIndex PlayerIndex { get; }

        public GamepadController(CommandManager commandManager, PlayerIndex playerIndex)
        {
            _commandManager = commandManager;
            PlayerIndex = playerIndex;
            _buttonCommandDictionary = new Dictionary<(Buttons button, bool oneShot), CommandType>
            {
                {new (Buttons.Start, true), CommandType.NavigableGridExecuteCommand},
                {new (Buttons.A, true), CommandType.PlayerAttackCommand},
                {new (Buttons.B, true), CommandType.PlayerCycleEquippableCommand},
                {new (Buttons.Y, true), CommandType.ToggleInventoryCommand},
                {new (Buttons.X, true), CommandType.PlayerUseEquippableCommand},
                {new (Buttons.None, false), CommandType.PlayerStandingCommand},
                {new (Buttons.LeftShoulder, true), CommandType.QuickSaveCommand},
                {new (Buttons.RightShoulder, true), CommandType.QuickLoadCommand},
                {new (Buttons.LeftTrigger, true), CommandType.MuteCommand},
                {new (Buttons.RightTrigger, true), CommandType.ToggleGizmosCommand},
                {new (Buttons.RightStick, true), CommandType.ResetCommand},
                {new (Buttons.LeftStick, true), CommandType.ExitCommand},
                {new (Buttons.DPadRight, true), CommandType.NavigableGridMoveCommand},
                {new (Buttons.DPadLeft, true), CommandType.NavigableGridMoveCommand},
                {new (Buttons.DPadDown, true), CommandType.NavigableGridMoveCommand},
                {new (Buttons.DPadUp, true), CommandType.NavigableGridMoveCommand},
            };

            _currentState = GamePad.GetState(PlayerIndex);
            _previousState = _currentState;
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = GamePad.GetState(PlayerIndex);

            // Handle thumbstick movement
            Vector2 leftThumbstick = _currentState.ThumbSticks.Left;

            if (leftThumbstick.Length() > 0.1f) // Ignore small movements (dead zone)
            {
                Keys keyboard_key_cast = GetDirection(leftThumbstick);
                _commandManager.Execute(CommandType.PlayerMoveCommand, keyboard_key_cast);
            }
            else
            {
                _commandManager.Execute(CommandType.PlayerStandingCommand, Buttons.None);
            }

            // Handle button-based commands
            foreach (var buttonCommandPair in _buttonCommandDictionary)
            {
                (Buttons button, bool oneShot) = buttonCommandPair.Key;

                if (!oneShot && _currentState.IsButtonDown(button))
                {
                    _commandManager.Execute(buttonCommandPair.Value, button);
                    break;
                }
                else if (oneShot && OneShotPressed(button))
                {
                        _commandManager.Execute(buttonCommandPair.Value, button);
                        break;
                }
            }
        }

        private bool OneShotPressed(Buttons button)
        {
            return _currentState.IsButtonDown(button) && !_previousState.IsButtonDown(button);
        }

        private static Keys GetDirection(Vector2 input)
        {
            // Ignore very small values (dead zone)
            if (input.Length() < 0.1f)
                return Keys.None;

            // Compare the absolute values of X and Y to determine direction
            if (Math.Abs(input.X) > Math.Abs(input.Y))
            {
                // X component is dominant
                return input.X > 0 ? Keys.D : Keys.A;
            }
            else
            {
                // Y component is dominant
                return input.Y > 0 ? Keys.W : Keys.S;
            }
        }
    }
}
