using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;


namespace MonoZelda.Link.Projectiles;

public class Arrow : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private bool rotate;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private Player player;

    public Arrow(SpriteDict projectileDict, Player player) : base(projectileDict, player)
    {
        this.projectileDict = projectileDict;
        this.player = player;
        Finished = false;
        rotate = false;
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
                rotate = false;
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * new Vector2(0, 1);
                SetProjectileSprite("arrow_green_down");
                rotate = false;
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * new Vector2(-1, 0);
                SetProjectileSprite("arrow_green_left");
                rotate = true;
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * new Vector2(1, 0);
                SetProjectileSprite("arrow_green_right");
                rotate = true;
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

    public void FinishProjectile()
    {
        tilesTraveled = 4;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        if (rotate)
        {
            return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 32 / 2, 64, 32);
        }
        else
        {
            return new Rectangle(spawnPosition.X - 32 / 2, spawnPosition.Y - 64 / 2, 32, 64);
        }
    }
}


