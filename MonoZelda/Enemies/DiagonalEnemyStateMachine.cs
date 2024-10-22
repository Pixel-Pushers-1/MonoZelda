using Microsoft.Xna.Framework;

namespace MonoZelda.Enemies;

public class DiagonalEnemyStateMachine
{
    public enum HorDirection
    {
        Left,
        Right,
        None
    }

    public enum VertDirection
    {
        Up,
        Down,
        None
    }

    private VertDirection CurrentVert { get; set; } = VertDirection.None;
    private HorDirection CurrentHor { get; set; } = HorDirection.None;

    public void ChangeVertDirection(VertDirection newVert)
    {
        CurrentVert = newVert;
    }

    public void ChangeHorDirection(HorDirection newHor)
    {
        CurrentHor = newHor;
    }

    public Point Update(Point position)
    {
        switch (CurrentVert)
        {
            case VertDirection.Up:
                position.Y -= 1;
                break;
            case VertDirection.Down:
                position.Y += 1;

                break;
        }

        switch (CurrentHor)
        {
            case HorDirection.Left:
                position.X -= 1;
                break;
            case HorDirection.Right:
                position.X += 1;
                break;
        }

        return position;
    }
}