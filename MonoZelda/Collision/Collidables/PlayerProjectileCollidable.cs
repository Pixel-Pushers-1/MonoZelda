using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Collision;

public class PlayerProjectileCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public ProjectileType projectileType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }
    public ProjectileManager ProjectileManager { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public PlayerProjectileCollidable(Rectangle bounds, ProjectileType projectileType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.PlayerProjectile;
        this.projectileType = projectileType;
    }

    public void UnregisterHitbox()
    {
        hitbox.Unregister();
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

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public void setProjectileManager(ProjectileManager projectileManager)
    {
        ProjectileManager = projectileManager;
    }
}
