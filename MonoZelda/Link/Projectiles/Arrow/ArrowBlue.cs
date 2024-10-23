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

    public ArrowBlue(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        rotate = false;
        tilesTraveled = 0;
        InitialPosition = SetInitialPosition(Dimension);
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

        string spriteName = $"arrow_blue_{playerDirection.ToString().ToLower()}";
        SetProjectileSprite(spriteName);

        rotate = (playerDirection == Direction.Left || playerDirection == Direction.Right);
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
        tilesTraveled = 6;
    }

    public Rectangle getCollisionRectangle()
    {
        Point spawnPosition = projectilePosition.ToPoint();
        int width = rotate ? 64 : 32;
        int height = rotate ? 32 : 64;

        return new Rectangle(spawnPosition.X - width / 2, spawnPosition.Y - height / 2, width, height);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < 5)
        {
            updatePosition();
        }
        else if (tilesTraveled == 5)
        {
            SetProjectileSprite("poof");
            tilesTraveled = 6;
        }
        else if (tilesTraveled == 6)
        {
            Finished = true;
            projectileDict.SetSprite("");
            projectileDict.Enabled = false;
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}
