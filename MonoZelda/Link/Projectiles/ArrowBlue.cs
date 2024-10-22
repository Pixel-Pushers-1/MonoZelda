using Microsoft.Xna.Framework;
using System;
using MonoZelda.Sprites;

namespace MonoZelda.Link.Projectiles;

public class ArrowBlue : Projectile, IProjectile
{
    private bool Finished;
    private float projectileSpeed = 4f;
    private int tilesTraveled;
    private bool rotate;
    private Vector2 InitialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;
    private Player player;

    public ArrowBlue(SpriteDict projectileDict, Player player) : base(projectileDict, player)
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
                SetProjectileSprite("arrow_blue_up");
                rotate = false;
                break;
            case Direction.Down:
                projectilePosition += projectileSpeed * new Vector2(0, 1);
                SetProjectileSprite("arrow_blue_down");
                rotate = false;
                break;
            case Direction.Left:
                projectilePosition += projectileSpeed * new Vector2(-1, 0);
                SetProjectileSprite("arrow_blue_left");
                rotate = true;
                break;
            case Direction.Right:
                projectilePosition += projectileSpeed * new Vector2(1, 0);
                SetProjectileSprite("arrow_blue_right");
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
        if (tilesTraveled < 5)
        {
            updatePosition();
            projectileDict.Position = projectilePosition.ToPoint();
            updateTilesTraveled();
        }
        else if (tilesTraveled == 5)
        {
            SetProjectileSprite("poof");
            tilesTraveled = 6;
        }
        else if (tilesTraveled == 6)
        {
            Finished = reachedDistance();
        }

    }

    public bool reachedDistance()
    {
        bool reachedDistance = false;

        if (tilesTraveled == 6)
        {
            reachedDistance = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }

        return reachedDistance;
    }

    public bool hasFinished()
    {
        return Finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = 6;
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
