using System;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Link;
using PixelPushers.MonoZelda.Controllers;
using Microsoft.Xna.Framework.Input;

namespace PixelPushers.MonoZelda.Commands;
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public class PlayerMoveCommand : ICommand
{
    private IController controller;
    private Vector2 scalarVector;
    private Direction playerDirection;
    private Player player; 

    public PlayerMoveCommand()
    {

    }

    public PlayerMoveCommand(Player player)
    {
    
        this.player = player; 
        SetPlayerDirection();
        
    }
    public Direction PlayerDirection
    {
        get 
        { 
            return playerDirection; 
        }
    }
    public Vector2 PlayerVector
    { 
        get 
        { 
            return scalarVector; 
        } 
    }

    private void SetPlayerDirection()
    {
        if (scalarVector.X > 0)
            playerDirection = Direction.Right;
        else if (scalarVector.X < 0)
            playerDirection = Direction.Left;
        else if (scalarVector.Y > 0)
            playerDirection = Direction.Down;
        else if (scalarVector.Y < 0)
            playerDirection = Direction.Up;
    }

    private void SetScalarVector(Keys PressedKey)
    {
        if (PressedKey == Keys.W || PressedKey == Keys.Up)
        {
            this.scalarVector = new Vector2(0, -1); // Move up
        }
        else if (PressedKey == Keys.S || PressedKey == Keys.Down)
        {
            this.scalarVector = new Vector2(0, 1); // Move down
        }
        else if (PressedKey == Keys.A || PressedKey == Keys.Left)
        {
            this.scalarVector = new Vector2(-1, 0); // Move left
        }
        else if (PressedKey == Keys.D || PressedKey == Keys.Right)
        {
            this.scalarVector = new Vector2(1, 0); // Move right
        }
        SetPlayerDirection();
    }

    public GameState Execute(Keys PressedKey)
    {
        // set scalar vector for player direction
        SetScalarVector(PressedKey);

        // call player move method
        if (player != null)
        {
            player.MovePlayer(this);
        }

        // Keep GameState the same inside the controller
        return controller.GameState;
    }
    public GameState UnExecute()
    {
        // Implement if you need to reverse this command
        throw new NotImplementedException();
    }

    public void SetController(IController controller)
    {
        this.controller = controller;
    }   
}
