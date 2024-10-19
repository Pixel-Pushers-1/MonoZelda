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

    public Point Update(Point position, GraphicsDeviceManager graphics)
    {
        switch (CurrentVert)
        {
            case VertDirection.Up:
                if (position.Y >= 0 + 32)
                {
                    position.Y -= 1;
                }

                break;
            case VertDirection.Down:
                if (position.Y <= graphics.PreferredBackBufferHeight - 32)
                {
                    position.Y += 1;
                }

                break;
        }

        switch (CurrentHor)
        {
            case HorDirection.Left:
                if (position.X >= 0 + 32)
                {
                    position.X -= 1;
                }

                break;
            case HorDirection.Right:
                if (position.X <= graphics.PreferredBackBufferWidth - 32)
                {
                    position.X += 1;
                }

                break;
        }

        return position;
    }
}