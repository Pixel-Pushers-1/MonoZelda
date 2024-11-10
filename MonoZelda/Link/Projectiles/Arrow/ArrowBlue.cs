using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Link.Projectiles;

public class ArrowBlue : ProjectileFactory, IProjectile
{
    private bool Finished;
    private const float PROJECTILE_SPEED = 8f;
    private const int TILES_TO_TRAVEL = 5;
    private int tilesTraveled;
    private bool rotate;
    private Vector2 initialPosition;
    private Vector2 Dimension = new Vector2(8, 16);
    private SpriteDict projectileDict;

    public ArrowBlue(SpriteDict projectileDict, Vector2 playerPosition, Direction playerDirection)
    : base(projectileDict, playerPosition, playerDirection)
    {
        this.projectileDict = projectileDict;
        Finished = false;
        rotate = false;
        tilesTraveled = 0;
        initialPosition = SetInitialPosition(Dimension);
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

        string spriteName = $"arrow_blue_{playerDirection.ToString().ToLower()}";
        SetProjectileSprite(spriteName);

        rotate = (playerDirection == Direction.Left || playerDirection == Direction.Right);
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
        int width = rotate ? 64 : 32;
        int height = rotate ? 32 : 64;

        return new Rectangle(spawnPosition.X - width / 2, spawnPosition.Y - height / 2, width, height);
    }

    public void UpdateProjectile()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            updatePosition();
        }
        else if (tilesTraveled == TILES_TO_TRAVEL)
        {
            SetProjectileSprite("poof");
            tilesTraveled = 6;
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
