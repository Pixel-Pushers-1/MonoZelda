using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Sprites;
using Microsoft.Xna.Framework;
using System;
using MonoZelda.Trigger;
using MonoZelda.Link;

namespace MonoZelda.Collision;

public class TriggerCollidable : ICollidable, ITrigger
{
    public CollidableType type { get; set; }
    public Rectangle Bounds { get; set; }
    public SpriteDict CollidableDict { get; set; }

    public Action<Direction> OnTrigger { get; set; }

    private readonly CollisionHitboxDraw hitbox;

    public TriggerCollidable(Rectangle bounds)
    {
        Bounds = bounds;
        hitbox = new CollisionHitboxDraw(this);
        type = CollidableType.Trigger;
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

    public void Trigger(Direction direction)
    {
        OnTrigger?.Invoke(direction);
    }
}
