using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class CandleBlue : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(16, 16);
    private SpriteDict projectileDict;
    private Player player;

    public CandleBlue(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
        SetProjectileSprite("fire");
    }

    private void updatePosition()
    {
        Vector2 directionVector = playerDirection switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            Direction.Right => new Vector2(1, 0),
            _ => Vector2.Zero
        };

        projectilePosition += projectileSpeed * directionVector;
        updateTilesTraveled();
    }
    private void updateTilesTraveled()
    {
        double tolerance = 0.000001;
        if (Math.Abs(CalculateDistance(InitialPosition) - 64f) < tolerance)
        {
            tilesTraveled++;
            InitialPosition = projectilePosition;
        }
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = 2;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < 2)
        {
            updatePosition();
        }
        else if (tilesTraveled == 2)
        {
            Finished = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}