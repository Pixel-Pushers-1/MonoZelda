using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class CandleBlue : ProjectileFactory, IProjectile
{
    private bool Finished;
    private const float PROJECTILE_SPEED = 4f;
    private const int TILES_TO_TRAVEL = 2;
    private int tilesTraveled;
    private Vector2 initialPosition;
    private Vector2 Dimension = new Vector2(16, 16);
    private SpriteDict projectileDict;
    private Player player;

    public CandleBlue(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        tilesTraveled = 0;
        initialPosition = SetInitialPosition(Dimension);
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

        projectilePosition += PROJECTILE_SPEED * directionVector;
        updateTilesTraveled();
    }
    private void updateTilesTraveled()
    {
        double distanceToTravel = 64f;
        double cumulativeDistance = Vector2.Distance(projectilePosition, initialPosition);

        if (cumulativeDistance >= distanceToTravel)
        {
            tilesTraveled++;
            initialPosition = projectilePosition;
        }
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = TILES_TO_TRAVEL;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            updatePosition();
        }
        else
        {
            Finished = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}