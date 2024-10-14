using Microsoft.Xna.Framework.Input;
using PixelPushers.MonoZelda.Commands;

namespace PixelPushers.MonoZelda.Controllers;

public class MouseController : IController
{
    private MouseState mouseState;
    private GameState gameState;
    private CommandManager commandManager;

    public MouseController(CommandManager commandManager)
    {
        gameState = GameState.Start;
        this.commandManager = commandManager;
    }

    // Properties
    public MouseState MouseState
    {
        get
        {
            return mouseState;
        }
        set
        {
            mouseState = value;
        }
    }

    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
        }
    }

    public bool Update()
    {
        MouseState = Mouse.GetState();
        GameState newState = gameState;

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