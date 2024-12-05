using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using MonoZelda.Enemies;
using MonoZelda.Link;

namespace MonoZelda.Collision;

public class PlayerCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }
    public PlayerCollisionManager PlayerCollision { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public PlayerCollidable(Rectangle bounds)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.Player;
    }

    public void setCollisionManager(PlayerCollisionManager playerCollision)
    {
        PlayerCollision = playerCollision;
    }

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public void UnregisterHitbox()
    {
        hitbox.Unregister();
    }

    public void HandleEnemyCollision(Direction collisionDirection)
    {
        PlayerCollision.HandleEnemyCollision(collisionDirection);
    }

    public bool Intersects(ICollidable other)
    {
        return Bounds.Intersects(other.Bounds);
    }

    public Rectangle GetIntersectionArea(ICollidable other)
    {
        return Rectangle.Intersect(Bounds, other.Bounds);
    }

    public override string ToString()
    {
        return $"{type}";
    }
}
