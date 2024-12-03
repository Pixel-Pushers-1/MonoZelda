using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Dungeons;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Shaders;
using System.Collections.Generic;

namespace MonoZelda.Link.Projectiles;

public class Fire : IProjectile
{
    private bool finished;
    private const float PROJECTILE_SPEED = 4f;
    private const int TILES_TO_TRAVEL = 2;
    private int tilesTraveled;
    private Vector2 initialPosition;
    private Vector2 projectilePosition;
    private ILight fireLight;
    private Direction projectileDirection;
    private List<ILight> lights;
    private CollisionController collisionController;
    private PlayerProjectileCollidable projectileCollidable;
    private SpriteDict projectileDict;

    public Vector2 ProjectilePosition
    {
        get { return projectilePosition; }
        set { projectilePosition = value; }
    }

    public Fire(Vector2 spawnPosition, CollisionController collisionController)
    {
        finished = false;
        tilesTraveled = 0;
        initialPosition = spawnPosition;
        projectileDirection = PlayerState.Direction;
        this.collisionController = collisionController;
    }

    private void updatePosition()
    {
        Vector2 directionVector = DungeonConstants.DirectionVector[projectileDirection];

        projectilePosition += PROJECTILE_SPEED * directionVector;
        projectileCollidable.Bounds = getCollisionRectangle();
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
        return new Rectangle(spawnPosition.X - 64 / 2, spawnPosition.Y - 64 / 2, 64, 64);
    }

    public void Setup(params object[] args)
    {
        // add light for fire
        lights = (List<ILight>)args[0];
        fireLight = new ProjectileLight(this);
        lights.Add(fireLight);

        // other collision, sprite dict setup
        projectilePosition = initialPosition;
        fireLight.Position = projectilePosition.ToPoint();
        SoundManager.PlaySound("LOZ_Candle", false);
        projectileDict = new SpriteDict(SpriteType.Projectiles, SpriteLayer.Projectiles, initialPosition.ToPoint());
        projectileDict.SetSprite("fire");
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.Fire);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void Update()
    {
        if (tilesTraveled < TILES_TO_TRAVEL)
        {
            updatePosition();
        }
        else
        {
            finished = true;
            lights.Remove(fireLight);
            projectileCollidable.UnregisterHitbox();
            projectileDict.Unregister();
            collisionController.RemoveCollidable(projectileCollidable);
        }
        projectileDict.Position = projectilePosition.ToPoint();
    }
}