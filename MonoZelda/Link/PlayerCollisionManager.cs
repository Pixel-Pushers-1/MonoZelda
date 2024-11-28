using Microsoft.Xna.Framework;
using MonoZelda.Collision;
using MonoZelda.Commands.GameCommands;
using MonoZelda.Controllers;
using MonoZelda.Dungeons;
using MonoZelda.Events;
using MonoZelda.Sound;
using MonoZelda.Sprites;
using System.Diagnostics;

namespace MonoZelda.Link;

public class PlayerCollisionManager
{
    private const float KNOCKBACK_FORCE = 8f;
    private const float KNOCKBACK_TIME = 0.2f;
    private const float INVULNERABILITY_TIME = 1f;

    private readonly int width;
    private readonly int height;
    private PlayerSpriteManager player;
    private PlayerTakeDamageCommand damageCommand;
    private PlayerCollidable playerHitbox;
    private Vector2 knockbackVelocity;
    private float knockbackTimer = 0;
    private float invulnerabilityTimer = 0;

    public PlayerCollisionManager(PlayerSpriteManager player, PlayerCollidable playerHitbox, CollisionController collisionController, PlayerTakeDamageCommand damageCommand) {
        this.player = player;
        this.damageCommand = damageCommand;
        this.playerHitbox = playerHitbox;
        this.width = 52;
        this.height = 52;

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

        if(PlayerState.Health <= 0)
        {
            player.DisablePlayerSprite();
            EventManager.TriggerLinkDeathAnimation();
        }

        if (knockbackTimer > 0)
        {
            ApplyKnockback();
            knockbackTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
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

    public void HandleClockCollision()
    {
        player.ClockFlash();
        invulnerabilityTimer = INVULNERABILITY_TIME*4f;
    }

    public void HandleBowCollision(SpriteDict bowDict)
    {
        bowDict.Position = player.GetPlayerPosition().ToPoint() + new Point(-32, -96);
        player.PickUpItem(PickUpType.pickupitem_onehand);
        invulnerabilityTimer = INVULNERABILITY_TIME * 3f;
    }

    public void HandleTriforceCollision()
    {
        player.DisablePlayerSprite();
    }

    public void HandleEnemyProjectileCollision(Direction collisionDirection)
    {
        if (invulnerabilityTimer > 0)
            return;

        if ((int)PlayerState.Direction + (int)collisionDirection != 0)
        {
            damageCommand.Execute();
            invulnerabilityTimer = INVULNERABILITY_TIME;
        }
        else
        {
            SoundManager.PlaySound("LOZ_Shield", false);
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

        if (invulnerabilityTimer > 0f)
            return;
        if (knockbackTimer <= 0)
        {
            Vector2 knockbackDirection = GetKnockbackDirection(collisionDirection)*-1;
            knockbackVelocity = knockbackDirection * KNOCKBACK_FORCE;
            knockbackTimer = KNOCKBACK_TIME;
            invulnerabilityTimer = INVULNERABILITY_TIME;
            ApplyKnockback();
            damageCommand.Execute();
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
        return DungeonConstants.DirectionVector[collisionDirection] * -1;
    }
}