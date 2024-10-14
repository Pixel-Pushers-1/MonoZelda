using System;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Link;
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
    private Vector2 scalarVector;
    private Direction playerDirection;
    private Player player; 

    public MonoZeldaGame Game { get; set; }

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
            scalarVector = new Vector2(0, -1); // Move up
        }
        else if (PressedKey == Keys.S || PressedKey == Keys.Down)
        {
            scalarVector = new Vector2(0, 1); // Move down
        }
        else if (PressedKey == Keys.A || PressedKey == Keys.Left)
        {
            scalarVector = new Vector2(-1, 0); // Move left
        }
        else if (PressedKey == Keys.D || PressedKey == Keys.Right)
        {
            scalarVector = new Vector2(1, 0); // Move right
        }
        SetPlayerDirection();
    }

    public void Execute(Keys PressedKey)
    {
        // set scalar vector for player direction
        SetScalarVector(PressedKey);

        // call player move method
        if (player != null)
        {
            player.MovePlayer(this);
        }
    }
    public void UnExecute()
    {
        // Implement if you need to reverse this command
        throw new NotImplementedException();
    } 
}
