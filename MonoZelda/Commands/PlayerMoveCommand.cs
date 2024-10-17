using MonoZelda.Link;
using Microsoft.Xna.Framework.Input;

namespace MonoZelda.Commands;

public class PlayerMoveCommand : ICommand
{
    private Player player; 

    public PlayerMoveCommand()
    {
        //empty
    }

    public PlayerMoveCommand(Player player)
    {
        this.player = player;
        PlayerDirection = Direction.Down;
    }

    public Direction PlayerDirection { get; private set; }

    public void Execute(params object[] metadata)
    {
        Keys pressedKey = (Keys) metadata[0];
        SetPlayerDirection(pressedKey);
        player?.Move(this);
    }

    public void UnExecute()
    {
        //empty
    }

    private void SetPlayerDirection(Keys pressedKey) {
        if (pressedKey == Keys.W || pressedKey == Keys.Up) {
            PlayerDirection = Direction.Up; 
        }
        else if (pressedKey == Keys.S || pressedKey == Keys.Down) {
            PlayerDirection = Direction.Down; 
        }
        else if (pressedKey == Keys.A || pressedKey == Keys.Left) {
            PlayerDirection = Direction.Left; 
        }
        else if (pressedKey == Keys.D || pressedKey == Keys.Right) {
            PlayerDirection = Direction.Right; 
        }
    }
}
