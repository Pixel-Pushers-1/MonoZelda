using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoZelda.UI.NavigableMenus;

namespace MonoZelda.Commands.GameCommands;

public class NavigableGridMoveCommand : ICommand
{
    private INavigableGrid grid;

    public NavigableGridMoveCommand() {
        //empty
    }

    public NavigableGridMoveCommand(INavigableGrid grid) {
        this.grid = grid;
    }

    public void Execute(params object[] metadata) {
        if (metadata[0] is Keys) {
            Keys pressedKey = (Keys) metadata[0];
            Point movement = GetMovement(pressedKey);
            grid?.MoveSelection(movement);
        } else {
            Buttons pressedButton = (Buttons) metadata[0];
            Point movement = GetMovementButton(pressedButton);
            grid?.MoveSelection(movement);
        }
    }

    public void UnExecute() {
        //empty
    }

    private static Point GetMovement(Keys pressedKey) {
        return pressedKey switch {
            Keys.W => new Point(0, -1),
            Keys.A => new Point(-1, 0),
            Keys.S => new Point(0, 1),
            Keys.D => new Point(1, 0),
            Keys.Up => new Point(0, -1),
            Keys.Left => new Point(-1, 0),
            Keys.Down => new Point(0, 1),
            Keys.Right => new Point(1, 0),
            _ => Point.Zero,
        };
    }

    private static Point GetMovementButton(Buttons pressedButton) {
        return pressedButton switch {
            Buttons.DPadUp => new Point(0, -1),
            Buttons.DPadLeft => new Point(-1, 0),
            Buttons.DPadDown => new Point(0, 1),
            Buttons.DPadRight => new Point(1,0),
            _ => Point.Zero
        };
    }
}
