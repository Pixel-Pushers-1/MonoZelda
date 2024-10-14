using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelPushers.MonoZelda.Collision.Collidables;

public class Collidable : ICollidable
{
    private Rectangle bounds;
    private readonly CollisionHitboxDraw hitbox;

    public Collidable(Rectangle bounds, GraphicsDevice graphicsDevice)
    {
        this.bounds = bounds;
        hitbox = new CollisionHitboxDraw(this, graphicsDevice);
    }

    public Rectangle Bounds
    {
        get
        {
            return bounds;
        }
        set
        {
            bounds = value;
        }
    }

    public bool Intersects(ICollidable other)
    {
        return bounds.Intersects(other.Bounds);
    }

    public Rectangle GetIntersectionArea(ICollidable other)
    {
        return Rectangle.Intersect(bounds, other.Bounds);
    }

    public void SetGizmoColor(Color color)
    {
        hitbox.GizmoColor = color;
    }

    public void SetGizmoThickness(int thickness)
    {
        hitbox.Thickness = thickness;
    }
}





