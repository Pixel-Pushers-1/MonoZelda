﻿
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Enemies.EnemyProjectiles;
using MonoZelda.Enemies;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace MonoZelda.Collision;

public class BoundaryCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }

    private readonly CollisionHitboxDraw hitbox;

    public BoundaryCollidable(Rectangle bounds, GraphicsDevice graphicsDevice, CollidableType type)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
        this.type = type;
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