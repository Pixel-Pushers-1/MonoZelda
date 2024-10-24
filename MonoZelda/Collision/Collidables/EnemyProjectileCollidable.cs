using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Collision;

public class EnemyProjectileCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; private set; }
    public IEnemyProjectile EnemyProjectile { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public EnemyProjectileCollidable(Rectangle bounds, GraphicsDevice graphicsDevice)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        type = CollidableType.EnemyProjectile;
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
    public void setEnemyProjectile(IEnemyProjectile enemyProjectile)
    {
        EnemyProjectile = enemyProjectile;
    }
}
