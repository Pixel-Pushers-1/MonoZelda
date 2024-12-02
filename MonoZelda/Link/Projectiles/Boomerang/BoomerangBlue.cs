using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Dungeons;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sound;

namespace MonoZelda.Link.Projectiles;

public class BoomerangBlue : IProjectile
{
    private bool finished;
    private const float PROJECTILE_SPEED = 8f;
    private const int TILES_TO_TRAVEL = 5;
    private int tilesTraveled;
    private Vector2 initialPosition;
    private Vector2 projectilePosition;
    private Direction projectileDirection;
    private CollisionController collisionController;
    private PlayerProjectileCollidable projectileCollidable;
    private SpriteDict projectileDict;
    private TrackReturn tracker;

    public BoomerangBlue(Vector2 spawnPosition, CollisionController collisionController)
    {
        finished = false;
        tilesTraveled = 0;
        initialPosition = spawnPosition;
        projectileDirection = PlayerState.Direction;
        tracker = TrackReturn.CreateInstance(this, PROJECTILE_SPEED);
        this.collisionController = collisionController;
    }

    private void Forward()
    {
        Vector2 directionVector = DungeonConstants.DirectionVector[PlayerState.Direction];

        projectilePosition += PROJECTILE_SPEED * directionVector;
        projectileCollidable.Bounds = getCollisionRectangle();
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
        return finished;
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

    public void Setup()
    {
        projectilePosition = initialPosition;
        SoundManager.PlaySound("LOZ_Arrow_Boomerang", false);
        projectileDict = new SpriteDict(SpriteType.Projectiles, SpriteLayer.Projectiles, initialPosition.ToPoint());
        projectileDict.SetSprite("blue_boomerang");
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.BoomerangBlue);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void Update()
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
            finished = true;
            projectileDict.Unregister();
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}