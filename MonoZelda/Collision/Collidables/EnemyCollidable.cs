using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;

namespace MonoZelda.Collision.Collidables;

public class EnemyCollidable : ICollidable
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }
    private readonly CollisionHitboxDraw hitbox;

    public EnemyCollidable(Rectangle bounds, GraphicsDevice graphicsDevice)
    {
        Bounds = bounds;
        type = CollidableType.Enemy;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
    }

    public void setSpriteDict(SpriteDict collidableDict)
    {
        CollidableDict = collidableDict;
    }

    public bool Intersects(ICollidable other)
    {
        return Bounds.Intersects(other.Bounds);
    }

    public Rectangle GetIntersectionArea(ICollidable other)
    {
        return Rectangle.Intersect(Bounds, other.Bounds);
    }
}

