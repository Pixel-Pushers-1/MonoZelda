using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class Boomerang : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 8);
    private Player player;
    private SpriteDict projectileDict;
    private TrackReturn tracker;

    public Boomerang(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection, Player player)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
        SetProjectileSprite("boomerang");
        UseTrackReturn();
    }

    private void UseTrackReturn()
    {
        tracker = TrackReturn.CreateInstance(this, player, projectileSpeed);
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

        projectilePosition += projectileSpeed * directionVector;
        updateTilesTraveled();
    }

    private void ReturnToPlayer()
    {
        tracker.CheckResetOrigin(projectilePosition);
        projectilePosition += tracker.getProjectileNextPosition();
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
        tilesTraveled = 3;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 32 / 2, 32, 32);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < 3)
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
