using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;

namespace PixelPushers.MonoZelda.Controllers;

public class MouseController : IController
{
    private CommandManager _commandManager;

    public MouseController(CommandManager commandManager)
    {
        this._commandManager = commandManager;
    }

    // Properties
    public MouseState MouseState { get; private set; }

    public void Update(GameTime gameTime)
    {
        MouseState = Mouse.GetState();

        // Mouse input logic goes here
    }

}