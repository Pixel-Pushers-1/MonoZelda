using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;


namespace MonoZelda.Link.Projectiles.Arrows;

public class Arrow : Projectile, IProjectile
{
    private bool Finished;
    private Vector2 InitialPosition;
    private SpriteDict projectileDict;
    private Player player;
    private float projectileSpeed = 4f;
    private Vector2 Dimension = new Vector2(8, 16);
    private int tilesTraveled;

    public Arrow(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
    }

    private void updatePosition()
    {
        switch (playerDirection)
        {
            case Direction.Up:
                projectilePosition += projectileSpeed * new Vector2(0, -1);
                SetProjectileSprite("arrow_green_up");
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * new Vector2(0, 1);
                SetProjectileSprite("arrow_green_down");
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * new Vector2(-1, 0);
                SetProjectileSprite("arrow_green_left");
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * new Vector2(1, 0);
                SetProjectileSprite("arrow_green_right");
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
        if (tilesTraveled < 3)
        {
            updatePosition();
            projectileDict.Position = projectilePosition.ToPoint();
            updateTilesTraveled();
        }
        else if (tilesTraveled == 3)
        {
            SetProjectileSprite("poof");
            tilesTraveled = 4;
        }
        else if (tilesTraveled == 4)
        {
            projectileDict.Enabled = false;
            Finished = reachedDistance();
        }

    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 4)
        {
            reachedDistance = true;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }
}


