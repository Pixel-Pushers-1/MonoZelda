using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;
using MonoZelda.Sound;
using MonoZelda.Sprites;

namespace MonoZelda.Link.Projectiles;

public class WoodenSword : IProjectile
{
    private bool finished;
    private int timer;
    private const int HITBOX_TIMER = 4;
    private bool rotate;
    private Vector2 initialPosition;
    private Direction projectileDirection;
    private Vector2 projectilePosition;
    private CollisionController collisionController;
    private PlayerProjectileCollidable projectileCollidable;
    private SpriteDict projectileDict;

    public Vector2 ProjectilePosition
    {
        get { return projectilePosition; }
        set { projectilePosition = value; }
    }

    public WoodenSword(Vector2 spawnPosition, CollisionController collisionController)
    {
        finished = false;
        rotate = false;
        timer = 0;
        projectileDirection = PlayerState.Direction;
        initialPosition = spawnPosition;
        this.collisionController = collisionController;
    }

    private void updateRotate()
    {
        if (projectileDirection == Direction.Right || projectileDirection == Direction.Left)
        {
            rotate = true;
        }
        projectileCollidable.Bounds = getCollisionRectangle();
    }

    public bool hasFinished()
{
        return finished;
    }

    public void FinishProjectile()
    {
        timer = HITBOX_TIMER;
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
        SoundManager.PlaySound("LOZ_Sword_Slash", false);
        projectileCollidable = new PlayerProjectileCollidable(getCollisionRectangle(), ProjectileType.WoodenSword);
        projectileCollidable.setProjectile(this);
        collisionController.AddCollidable(projectileCollidable);
    }

    public void Update()
    {
        if (timer < HITBOX_TIMER)
        {
            timer++;
            updateRotate();
        }
        else
        {
            finished = true;
            projectileCollidable.UnregisterHitbox();
            collisionController.RemoveCollidable(projectileCollidable);
        }
    }
}
