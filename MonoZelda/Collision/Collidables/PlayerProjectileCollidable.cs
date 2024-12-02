using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoZelda.Controllers;

namespace MonoZelda.Collision;

public class PlayerProjectileCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public ProjectileType projectileType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }
    public IProjectile Projectile { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public PlayerProjectileCollidable(Rectangle bounds, ProjectileType projectileType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.PlayerProjectile;
        this.projectileType = projectileType;
    }

    public void HandleCollision()
    {
        Projectile.FinishProjectile();
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

    public void setProjectile(IProjectile projectile)
    {
        Projectile = projectile;
    }
}
