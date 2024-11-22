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
        private readonly List<Buttons> _pressedDirectionalButtons;
        private GamePadState _currentState;
        private GamePadState _previousState;

        public PlayerIndex PlayerIndex { get; }

        public GamepadController(CommandManager commandManager, PlayerIndex playerIndex)
        {
            _commandManager = commandManager;
            PlayerIndex = playerIndex;
            _pressedDirectionalButtons = new List<Buttons>();
            _buttonCommandDictionary = new Dictionary<(Buttons button, bool oneShot), CommandType>
            {
                {new (Buttons.Start, true), CommandType.StartGameCommand},
                {new (Buttons.Back, true), CommandType.ExitCommand},
                {new (Buttons.A, true), CommandType.PlayerAttackCommand},
                {new (Buttons.B, true), CommandType.PlayerFireProjectileCommand},
                {new (Buttons.Y, true), CommandType.ToggleInventoryCommand},
                {new (Buttons.LeftShoulder, true), CommandType.PlayerCycleProjectileCommand},
                {new (Buttons.RightShoulder, true), CommandType.PlayerCycleProjectileCommand},
                {new (Buttons.None, false), CommandType.PlayerStandingCommand}
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
                    if (buttonCommandPair.Value != CommandType.PlayerCycleProjectileCommand)
                    {
                        _commandManager.Execute(buttonCommandPair.Value, button);
                        break;
                    }
                    else if (button == Buttons.LeftShoulder)
                    {
                        _commandManager.Execute(buttonCommandPair.Value, -1);
                    }
                    else
                    {
                        _commandManager.Execute(buttonCommandPair.Value, 1);
                    }
                    
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
