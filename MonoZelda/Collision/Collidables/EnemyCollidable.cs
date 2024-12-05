using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Commands;

namespace MonoZelda.Collision;

public class EnemyCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public EnemyList enemyType { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }
    public EnemyCollisionManager EnemyCollision { get; private set; }

    private readonly CollisionHitboxDraw hitbox;

    public EnemyCollidable(Rectangle bounds, EnemyList enemyType)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.Enemy;
        this.enemyType = enemyType;
    }

    public void setCollisionManager(EnemyCollisionManager enemyCollision)
    {
        EnemyCollision = enemyCollision;
    }

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public Enemy getEnemy()
    {
        return EnemyCollision.Enemy;
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
}
