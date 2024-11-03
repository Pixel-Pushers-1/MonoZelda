using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Link;

public class PlayerCollisionManager
{
    private readonly int width;
    private readonly int height;
    private PlayerSpriteManager player;
    private PlayerCollidable playerHitbox;
    private const float KNOCKBACK_FORCE = 8f;
    private Vector2 knockbackVelocity;
    private int knockbackFramesRemaining = 0;
    private int invulnerabilityFramesRemaining = 0;

    public PlayerCollisionManager(PlayerSpriteManager player, PlayerCollidable playerHitbox, CollisionController collisionController)
    {
        this.player = player;
        this.playerHitbox = playerHitbox;
        this.width = 64;
        this.height = 64;

        Vector2 playerPosition = player.GetPlayerPosition();
        Rectangle bounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );
        playerHitbox.Bounds = bounds;
        playerHitbox.setCollisionManager(this);
    }

    public void Update()
    {
        UpdateBoundingBox();

        if (knockbackFramesRemaining > 0)
        {
            ApplyKnockback();
            knockbackFramesRemaining--;
        }

        if (invulnerabilityFramesRemaining > 0)
        {
            invulnerabilityFramesRemaining--;
        }
    }

    private void UpdateBoundingBox()
    {
        Vector2 playerPosition = player.GetPlayerPosition();
        Rectangle newBounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );
        playerHitbox.Bounds = newBounds;
    }

    public void HandleEnemyProjectileCollision(Direction collisionDirection)
    {
        if (invulnerabilityFramesRemaining > 0) return;

        if ((int)player.PlayerDirection + (int)collisionDirection == 0)
        {
            player.TakeDamage();
            invulnerabilityFramesRemaining = 60;
        }
    }

    public void HandleStaticCollision(Direction collisionDirection, Rectangle intersection)
    {
        Vector2 currentPos = player.GetPlayerPosition();
        switch (collisionDirection)
        {
            case Direction.Left:
                currentPos.X -= intersection.Width;
                break;
            case Direction.Right:
                currentPos.X += intersection.Width;
                break;
            case Direction.Up:
                currentPos.Y -= intersection.Height;
                break;
            case Direction.Down:
                currentPos.Y += intersection.Height;
                break;
        }
        player.SetPosition(currentPos);
    }

    public void HandleEnemyCollision(Direction collisionDirection)
    {
        if (invulnerabilityFramesRemaining > 0) return;

        if (knockbackFramesRemaining == 0)
        {
            Vector2 currentPos = player.GetPlayerPosition();
            Vector2 knockbackDirection = GetKnockbackDirection(collisionDirection);
            knockbackVelocity = knockbackDirection * KNOCKBACK_FORCE;
            knockbackFramesRemaining = 12;
            ApplyKnockback();
            player.TakeDamage();
            invulnerabilityFramesRemaining = 60;
        }
    }

    private void ApplyKnockback()
    {
        Vector2 currentPos = player.GetPlayerPosition();
        Vector2 newPosition = currentPos + knockbackVelocity;
        player.SetPosition(newPosition);
    }

    private Vector2 GetKnockbackDirection(Direction collisionDirection)
    {
        return collisionDirection switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            Direction.Right => new Vector2(1, 0),
            _ => Vector2.Zero
        };
    }
}