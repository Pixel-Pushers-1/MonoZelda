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

    public CandleBlue(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        SetProjectileSprite("fire");
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void updatePosition()
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
        if (tilesTraveled < 2)
        {
            updatePosition();
            projectileDict.Position = projectilePosition.ToPoint();
            updateTilesTraveled();
        }
        else if (tilesTraveled == 2)
        {
            projectileDict.Enabled = false;
            Finished = reachedDistance();
        }
    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 2)
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
        return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64);
    }
}