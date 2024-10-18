using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;

namespace MonoZelda.Link.Projectiles;

public class BoomerangBlue : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 8);
    private SpriteDict projectileDict;
    private Player player;
    private TrackReturn tracker;

    public BoomerangBlue(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        tilesTraveled = 0;
        SetProjectileSprite("boomerang_blue");
        InitialPosition = SetInitialPosition(Dimension);
        UseTrackReturn();
    }

    private void UseTrackReturn()
    {
        tracker = TrackReturn.CreateInstance(this, player, projectileSpeed);
    }

    private void Forward()
    {
        switch (playerDirection)
        {
            case Direction.Up:
                projectilePosition += projectileSpeed * new Vector2(0, -1);
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * new Vector2(0, 1);
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * new Vector2(-1, 0);
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * new Vector2(1, 0);
                break;
        }
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
    public void UpdateProjectile()
    {
        if (tilesTraveled < 5)
        {
            Forward();
        }
        else if (!reachedDistance())
        {
            ReturnToPlayer();
        }
        else
        {
            Finished = reachedDistance();
            projectileDict.Enabled = false;
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;
        if (tracker.Returned(projectilePosition))
        {
            reachedDistance = true;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 32 / 2, 32, 32);
    }
}