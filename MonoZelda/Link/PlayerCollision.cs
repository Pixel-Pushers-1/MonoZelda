using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Collision.Collidables;

namespace PixelPushers.MonoZelda.Link;

public class PlayerCollision
{
    private readonly int width;
    private readonly int height;
    private Player player;
    private Collidable playerHitbox;
    private CollidablesManager collidablesManager;
    public PlayerCollision(Player player, Collidable playerHitbox, CollidablesManager collidablesManager)
    {
        this.player = player;
        this.playerHitbox = playerHitbox;
        width = 64;
        height = 64;

        // Create initial hitbox for the player
        Vector2 playerPosition = player.getPlayerPosition();
        Rectangle bounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );

        this.collidablesManager = collidablesManager;
        playerHitbox.Bounds = bounds;
    }

    public void Update()
    {
        UpdateBoundingBox();
        CheckCollision();
    }

    private void UpdateBoundingBox()
    {
        Vector2 playerPosition = player.getPlayerPosition();
        Rectangle newBounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );

        playerHitbox.Bounds = newBounds;
    }

    private void CheckCollision()
    {
        bool collided = false;
        List<Collidable> collidableObjects = collidablesManager.GetListOfCollidableObjects();
        foreach (var hitbox in collidableObjects)
        {
            //ignore player's own hitbox
            if (hitbox == playerHitbox)
                continue;
            //collision has occurred
            if (playerHitbox.Intersects(hitbox))
            {
                collided = true;
                playerHitbox.SetGizmoColor(Color.Lime);
                hitbox.SetGizmoColor(Color.Lime);
            }
            else
            {
                //reset other collider's hitbox color
                hitbox.SetGizmoColor(Color.Red);
            }
        }
        //collided with nothing
        if (!collided)
        {
            playerHitbox.SetGizmoColor(Color.Red);
        }
    }
}
