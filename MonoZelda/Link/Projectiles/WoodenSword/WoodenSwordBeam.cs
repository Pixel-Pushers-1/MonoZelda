using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Link.Projectiles;

public class WoodenSwordBeam : IProjectile
{
    private bool finished;
    private float PROJECTILE_SPEED = 8f;
    private const int TILES_TO_TRAVEL = 5;
    private int tilesTraveled;
    private bool rotate;
    private Vector2 initialPosition;
    private Vector2 projectilePosition;
    private Direction projectileDirection;
    private CollisionController collisionController;
    private PlayerProjectileCollidable projectileCollidable;
    private SpriteDict projectileDict;

    public Vector2 ProjectilePosition
    {
        get { return projectilePosition; }
        set { projectilePosition = value; }
    }

    public WoodenSwordBeam(Vector2 spawnPosition, CollisionController collisionController)
    {
        finished = false;
        rotate = false;
        tilesTraveled = 0;
        projectileDirection = PlayerState.Direction;
        initialPosition = spawnPosition;
        this.collisionController = collisionController;
    }

    private void updatePosition()
    {
        Vector2 directionVector = DungeonConstants.DirectionVector[projectileDirection];

        rotate = (projectileDirection == Direction.Left || projectileDirection == Direction.Right);

        projectilePosition += PROJECTILE_SPEED * directionVector;
        projectileCollidable.Bounds = getCollisionRectangle();

        string spriteName = $"woodensword_item_{projectileDirection.ToString().ToLower()}";
        projectileDict.SetSprite(spriteName);

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
        return finished;
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

    public void Setup(params object[] args)
    {
        projectilePosition = initialPosition;
        SoundManager.PlaySound("LOZ_Sword_Shoot", false);
        projectileDict = new SpriteDict(SpriteType.Projectiles, SpriteLayer.Projectiles, projectilePosition.ToPoint());
        string spriteName = $"woodensword_item_{projectileDirection.ToString().ToLower()}";
        projectileDict.SetSprite(spriteName);
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.WoodenSwordBeam);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void Update()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            updatePosition();
        }
        else if (tilesTraveled == TILES_TO_TRAVEL)
        {
            projectileDict.SetSprite("poof");
            tilesTraveled = 6;
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



