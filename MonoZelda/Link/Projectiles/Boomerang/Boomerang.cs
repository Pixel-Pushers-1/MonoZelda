using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class Boomerang : ProjectileFactory, IProjectile
{
    private bool Finished;
    private const float PROJECTILE_SPEED = 6f;
    private const int TILES_TO_TRAVEL = 3;
    private int tilesTraveled;
    private Vector2 initialPosition;
    private Vector2 Dimension = new Vector2(8, 8);
    private PlayerSpriteManager player;
    private SpriteDict projectileDict;
    private TrackReturn tracker;

    public Boomerang(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection, PlayerSpriteManager player)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        tilesTraveled = 0;
        initialPosition = SetInitialPosition(Dimension);
        SetProjectileSprite("boomerang");
        UseTrackReturn();
    }

    private void UseTrackReturn()
    {
        tracker = TrackReturn.CreateInstance(this, player, PROJECTILE_SPEED);
    }

    private void Forward()
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

    private void ReturnToPlayer()
    {
        tracker.CheckResetOrigin(projectilePosition);
        projectilePosition += tracker.getProjectileNextPosition();
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
        return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 32 / 2, 32, 32);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            Forward();
        }
        else if (!tracker.Returned(projectilePosition))
        {
            ReturnToPlayer();
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
