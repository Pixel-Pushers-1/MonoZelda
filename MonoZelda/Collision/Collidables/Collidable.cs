using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;

namespace MonoZelda.Collision;

public class Collidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; private set; }
    public IEnemy Enemy { get; private set; }   
    public ProjectileManager ProjectileManager { get; private set; }

    public IEnemyProjectile EnemyProjectile { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public Collidable(Rectangle bounds, GraphicsDevice graphicsDevice, CollidableType type)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        this.type = type;
    }

    public void UnregisterHitbox() {
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

    public void setEnemy(IEnemy enemy)
    {
        Enemy = enemy;
    }

    public void setEnemyProjectile(IEnemyProjectile enemyProjectile)
    {
        EnemyProjectile = enemyProjectile;
    }

    public void setProjectileManager(ProjectileManager projectileManager)
    {
        ProjectileManager = projectileManager;
    }
}
