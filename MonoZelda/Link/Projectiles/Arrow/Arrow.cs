using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Dungeons;
using MonoZelda.Collision;

namespace MonoZelda.Link.Projectiles;

public class Arrow : IProjectile
{
    private const float PROJECTILE_SPEED = 8f;
    private const int TILES_TO_TRAVEL = 3;
    private bool finished;
    private int tilesTraveled;
    private bool rotate;
    private CollisionController collisionController;
    private SpriteDict projectileDict;
    private PlayerProjectileCollidable projectileCollidable;
    private Direction projectileDirection;
    private Vector2 projectilePosition;
    private Vector2 initialPosition;

    public Vector2 ProjectilePosition
    {
        get { return projectilePosition; }
        set { projectilePosition = value; }
    }

    public Arrow(Vector2 spawnPos, CollisionController collisionController)
    {
        finished = false;
        rotate = false;
        tilesTraveled = 0;
        projectileDirection = PlayerState.Direction;
        initialPosition = spawnPos;
        this.collisionController = collisionController;
    }

    private void updatePosition()
    {
        Vector2 directionVector = DungeonConstants.DirectionVector[projectileDirection];

        rotate = (projectileDirection == Direction.Left || projectileDirection == Direction.Right);

        projectilePosition += PROJECTILE_SPEED * directionVector;
        projectileCollidable.Bounds = getCollisionRectangle();

        string spriteName = $"arrow_green_{projectileDirection.ToString().ToLower()}";
        projectileDict.SetSprite(spriteName);

        updateTilesTraveled();
    }

    private void updateTilesTraveled()
    {
        double distanceToTravel = 64f;
        double cumulativeDistance = Vector2.Distance(projectilePosition,initialPosition);

        if (cumulativeDistance >= distanceToTravel)
        {
            tilesTraveled++;
            initialPosition = projectilePosition;
        }
    }

    public bool hasFinished()
    {
        return finished;
    }

    public void FinishProjectile()
    {
        tilesTraveled = TILES_TO_TRAVEL;
    }

    public Rectangle getCollisionRectangle()
    {
        Point rectPosition = projectilePosition.ToPoint();
        int width = rotate ? 64 : 32;
        int height = rotate ? 32 : 64;

        return new Rectangle(rectPosition.X - width / 2, rectPosition.Y - height / 2, width, height);
    }

    public void Setup(params object[] args)
    {
        projectilePosition = initialPosition;
        SoundManager.PlaySound("LOZ_Arrow_Boomerang", false);
        projectileDict = new SpriteDict(SpriteType.Projectiles, SpriteLayer.Projectiles, initialPosition.ToPoint());
        string spriteName = $"arrow_green_{projectileDirection.ToString().ToLower()}";
        projectileDict.SetSprite(spriteName);
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.Arrow);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void Update()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            updatePosition();
        }
        else if (tilesTraveled == (TILES_TO_TRAVEL))
        {
            projectileDict.SetSprite("poof");
            tilesTraveled = 4;
        }
        else
        {
            finished = true;
            projectileDict.Unregister();
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}


