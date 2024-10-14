using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;

namespace PixelPushers.MonoZelda.Controllers;

public class MouseController : IController
{
    private MouseState _mouseState;
    private CommandManager _commandManager;

    public MouseController(CommandManager commandManager)
    {
        this._commandManager = commandManager;
    }

    // Properties
    public MouseState MouseState
    {
        get
        {
            return _mouseState;
        }
        set
        {
            _mouseState = value;
        }
    }

    public void Update(GameTime gameTime)
    {
        MouseState = Mouse.GetState();

        // Mouse input logic goes here
        if (MouseState.LeftButton == ButtonState.Pressed)
        {
            newState = commandManager.Execute(CommandEnum.LoadRoomCommand, Keys.None);
        }

        // Setting new Game State of mouse controller if needed
        if (gameState != newState)
        {
            gameState = newState;
            return true;
        }
        return false;
    }

}