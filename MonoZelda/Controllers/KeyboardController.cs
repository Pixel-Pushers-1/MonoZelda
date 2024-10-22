using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;
using System.Collections.Generic;
using System.Linq;
namespace MonoZelda.Controllers;

public class KeyboardController : IController
{
    private readonly CommandManager _commandManager;
    private readonly Dictionary<Keys, (CommandType command, bool oneShot)> _keyBindings;
    private readonly Dictionary<Keys, double> _keyPressTimestamps;
    private double _currentTimestamp;

    public KeyboardController(CommandManager commandManager)
    {
        _commandManager = commandManager;
        _keyBindings = new Dictionary<Keys, (CommandType, bool)>();
        _keyPressTimestamps = new Dictionary<Keys, double>();
        _currentTimestamp = 0;
        // boolean passed in if is one shot 
        AddBinding(Keys.Enter, CommandType.StartGameCommand, true);
        AddBinding(Keys.W, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.Up, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.S, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.Down, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.D, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.Right, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.A, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.Left, CommandType.PlayerMoveCommand, false);
        AddBinding(Keys.G, CommandType.ToggleGizmosCommand, true);
        AddBinding(Keys.D1, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.D2, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.D3, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.D4, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.D5, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.D6, CommandType.PlayerUseItemCommand, true);
        AddBinding(Keys.T, CommandType.PlayerFireSwordBeam, true);
        AddBinding(Keys.Z, CommandType.PlayerAttackCommand, true);
        AddBinding(Keys.N, CommandType.PlayerAttackCommand, true);
        AddBinding(Keys.Q, CommandType.ExitCommand, true);
        AddBinding(Keys.R, CommandType.ResetCommand, true);
        AddBinding(Keys.None, CommandType.PlayerStandingCommand, false);
    }

    public KeyboardState CurrentKeyboardState { get; private set; }
    public KeyboardState PreviousKeyboardState { get; private set; }

    private void AddBinding(Keys key, CommandType command, bool oneShot)
    {
        _keyBindings[key] = (command, oneShot);
    }

    public void Update(GameTime gameTime)
    {
        _currentTimestamp += gameTime.ElapsedGameTime.TotalMilliseconds;
        CurrentKeyboardState = Keyboard.GetState();

        foreach (var key in _keyBindings.Keys)
        {
            if (key != Keys.None &&
                CurrentKeyboardState.IsKeyDown(key) &&
                (!_keyPressTimestamps.ContainsKey(key) || !PreviousKeyboardState.IsKeyDown(key)))
            {
                _keyPressTimestamps[key] = _currentTimestamp;
            }
        }

        foreach (var key in _keyPressTimestamps.Keys.ToList())
        {
            if (CurrentKeyboardState.IsKeyUp(key))
            {
                _keyPressTimestamps.Remove(key);
            }
        }

        Keys activeKey = Keys.None;

        if (_keyPressTimestamps.Any())
        {
            activeKey = _keyPressTimestamps
                .OrderByDescending(kvp => kvp.Value)
                .First()
                .Key;

            var (command, oneShot) = _keyBindings[activeKey];

            if (!oneShot || OneShotPressed(activeKey))
            {
                _commandManager.Execute(command, activeKey);
            }
        }
        else
        {
            _commandManager.Execute(CommandType.PlayerStandingCommand, Keys.None);
        }

        PreviousKeyboardState = CurrentKeyboardState;
    }

    private bool OneShotPressed(Keys key)
    {
        return CurrentKeyboardState.IsKeyDown(key) && !PreviousKeyboardState.IsKeyDown(key);
    }
}