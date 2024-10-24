using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Controllers;

namespace MonoZelda.Link;

public class PlayerCollision
{
    private readonly int width;
    private readonly int height;
    private Player player;
    private ICollidable playerHitbox;
    private CollisionController collisionController;
    private const float KNOCKBACK_FORCE = 30f;
    private Vector2 knockbackVelocity;

    public PlayerCollision(Player player, ICollidable playerHitbox, CollisionController collisionController)
    {
        this.player = player;
        this.playerHitbox = playerHitbox;
        this.width = 64;
        this.height = 64;

        // Create initial hitbox for the player
        Vector2 playerPosition = player.GetPlayerPosition();
        Rectangle bounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );
        this.collisionController = collisionController;

        playerHitbox.Bounds = bounds;
    }

    public void Update()
    {
        UpdateBoundingBox();
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
        Vector2 currentPos = player.GetPlayerPosition();
        Vector2 knockbackDirection = GetKnockbackDirection(collisionDirection);
        knockbackVelocity = knockbackDirection * KNOCKBACK_FORCE;
        ApplyKnockback();
        player.TakeDamage();
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
