using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Collision;

public class EnemyCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public EnemyList enemyType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; private set; }
    public IEnemy Enemy { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public EnemyCollidable(Rectangle bounds, GraphicsDevice graphicsDevice, EnemyList enemyType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        type = CollidableType.Enemy;
        this.enemyType = enemyType;
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

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public void setEnemy(IEnemy enemy)
    {
        Enemy = enemy;
    }

    public override string ToString()
    {
        return $"{type}";
    }

}
