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
    }

}