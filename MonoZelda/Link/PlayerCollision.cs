using Microsoft.Xna.Framework;
using PixelPushers.MonoZelda.Collision;
using PixelPushers.MonoZelda.Controllers;

namespace PixelPushers.MonoZelda.Link
{
    public class PlayerCollision
    {
        private readonly int width;
        private readonly int height;
        private Player player;
        private Collidable playerHitbox;
        private CollisionController collisionController;
        public PlayerCollision(Player player, Collidable playerHitbox, CollisionController collisionController)
        {
            this.player = player;
            this.playerHitbox = playerHitbox;
            this.width = 64;
            this.height = 64;

            // Create initial hitbox for the player
            Vector2 playerPosition = player.getPlayerPosition();
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
        Vector2 playerPosition = player.getPlayerPosition();
        Rectangle newBounds = new Rectangle(
            (int)playerPosition.X - width / 2,
            (int)playerPosition.Y - height / 2,
            width,
            height
        );

            playerHitbox.Bounds = newBounds;            
        }
    }
}