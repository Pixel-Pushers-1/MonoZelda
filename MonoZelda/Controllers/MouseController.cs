using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoZelda.Commands;

namespace MonoZelda.Controllers;

public class MouseController : IController
{
    private CommandManager _commandManager;

    public MouseController(CommandManager commandManager)
    {
        this._commandManager = commandManager;
    }

    // Properties
    public MouseState CurrentMouseState { get; private set; }
    public MouseState PreviousMouseState { get; private set; }

    public void Update(GameTime gameTime)
    {
        CurrentMouseState = Mouse.GetState();

        // Mouse input logic goes here
        if (OneShotPressed(MouseButton.Left))
        {
            _commandManager.Execute(CommandType.LoadRoomCommand, CurrentMouseState);
        }

        PreviousMouseState = CurrentMouseState;
    }

    private bool OneShotPressed(MouseButton key)
    {
        // Why does MouseState not have GetKey() for button :( -js
        switch (key)
        {
            case MouseButton.Left:
                return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentMouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentMouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
            default:
                return false;
        }
    }

    private enum MouseButton
    {
        Left,
        Right,
        Middle
    }
}